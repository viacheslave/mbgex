using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SpringOff.MGBEx
{
	internal sealed class ExportService
	{
		private readonly IApiService _apiService;
		private readonly Microsoft.Extensions.Logging.ILogger _logger;

		private readonly DateTime _stamp = DateTime.Now;

		public ExportService(IApiService apiService, ILoggerFactory loggerFactory)
		{
			_apiService = apiService;
			_logger = loggerFactory.CreateLogger<ExportService>();
		}

		public async Task Dump(LoginRequest loginRequest)
		{
			if (loginRequest == null)
				throw new ArgumentNullException(nameof(loginRequest));

			var session = await _apiService.Login(loginRequest);
			if (session == null)
				return;

			Dump(session, $"mgb.session.json");

			var userProfile = JsonConvert.DeserializeObject<UserProfile>(session);
			if (userProfile == null)
				return;

			var accountsData = await _apiService.GetAccounts(userProfile);
			Dump(accountsData, $"mgb.accounts.json");

			await DumpBillings(userProfile, 2, "waiting");
			await DumpBillings(userProfile, 5, "complete");
			await DumpBillings(userProfile, 6, "rejected");

			/*
      var userAccounts = JsonConvert.DeserializeObject<Dictionary<string, object>>(accountsData);

      foreach(var kvp in userAccounts)
      {
        int id;
        if (int.TryParse(kvp.Key, out id))
        {
          var account = JsonConvert.DeserializeObject<UserAccount>(kvp.Value.ToString());
          await DumpAccount(account, userProfile);
        }
      }*/
		}

		private async Task DumpBillings(UserProfile profile, int status, string label)
		{
			var billingsData = await _apiService.GetBillings(status, profile);
			if (billingsData == null)
				return;

			Dump(billingsData, $"mgb.flatBilling.{label}.json");

			var billings = JsonConvert.DeserializeObject<BillingCollection>(billingsData);
			foreach (var order in billings.Billings.SelectMany(b => b.Orders))
			{
				var billingData = await _apiService.GetBillingOrder(order.Id, profile);
				Dump(billingData, $"mgb.flatBilling.{label}.{order.Id}.json");
			}
		}

		private async Task DumpAccount(UserAccount account, UserProfile profile)
		{
			var flatInfoData = await _apiService.GetFlatInfo(account.Id, profile);
			if (flatInfoData == null)
				return;

			Dump(flatInfoData, $"mgb.flatInfo.{account.AccountId}.json");

			var monthListData = await _apiService.GetFlatMonthList(account.Id, profile);
			if (monthListData == null)
				return;

			Dump(monthListData, $"mgb.monthList.{account.AccountId}.json");

			var monthList = JsonConvert.DeserializeObject<FlatMonths>(monthListData);
			foreach (var flatMonth in monthList.Months)
			{
				var monthData = await _apiService.GetFlatInfoByMonth(account.Id, profile, flatMonth.Id);
				Dump(monthData, $"mgb.{account.AccountId}.{flatMonth.Id}.json");
			}
		}

		private void Dump(string data, string filename)
		{
			var exportFolder = CreateExportFolderIsNotExists();
			var filePath = Path.Combine(exportFolder, filename);
			File.AppendAllText(filePath, JsonConvert.DeserializeObject(data).ToString());
		}

		private string CreateExportFolderIsNotExists()
		{
			var currentFolder = Environment.CurrentDirectory;
			var exportFolder = Path.Combine(currentFolder, Path.Combine("export", _stamp.ToString("yyyyMMddHHmmss")));

			if (!Directory.Exists(exportFolder))
				Directory.CreateDirectory(exportFolder);

			return exportFolder;
		}
	}
}

