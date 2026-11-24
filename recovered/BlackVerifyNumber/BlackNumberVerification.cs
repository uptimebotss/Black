using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BlackVerifyNumber;

public class BlackNumberVerification
{
	private string PHONE_URL = "https://discord.com/api/v8/users/@me/phone";

	private string VERIFY_URL = "https://discord.com/api/v8/users/@me/phone/verify";

	private string USER_URL = "https://discord.com/api/v8/users/@me";

	private string GET_NUMB = "https://sms-activate.ru/stubs/handler_api.php";

	private string _token;

	public BlackNumberVerification(string api_key)
	{
		_token = api_key;
	}

	public NumberService GetService()
	{
		return NumberService.SmsActivate;
	}

	public async Task<bool> IsPhoneVerified(string token, bool useProxy = false, WebProxy proxy = null)
	{
		HttpClient client;
		if (!useProxy)
		{
			client = new HttpClient();
		}
		else if (useProxy && proxy != null)
		{
			HttpClientHandler handl = new HttpClientHandler();
			handl.Proxy = proxy;
			handl.UseProxy = true;
			client = new HttpClient((HttpMessageHandler)(object)handl);
		}
		else
		{
			client = new HttpClient();
		}
		HttpRequestMessage val = new HttpRequestMessage
		{
			RequestUri = new Uri(USER_URL)
		};
		((HttpHeaders)val.Headers).Add("Authorization", token ?? "");
		val.Method = HttpMethod.Get;
		HttpRequestMessage _fetch = val;
		HttpResponseMessage __ = await client.SendAsync(_fetch);
		if (__.StatusCode == HttpStatusCode.OK)
		{
			dynamic _r = JObject.Parse(await __.Content.ReadAsStringAsync());
			if (_r.phone == null)
			{
				return false;
			}
			return true;
		}
		throw new Exception("Something went wrong while trying to fetch /api/v8/users/@me");
	}

	private async Task<string> GetNumber()
	{
		HttpClient client = new HttpClient();
		string _re = await (await client.GetAsync(GET_NUMB + "?api_key=" + _token + "&action=getNumber&service=ds&operator=any&ref=734252&country=ru")).Content.ReadAsStringAsync();
		if (_re.Contains("ACCESS_NUMBER"))
		{
			return _re.Split(':')[1] + " " + _re.Split(':')[2];
		}
		return "ERROR";
	}

	public async Task<bool> Verify(string token, bool useProxy = false, WebProxy proxy = null)
	{
		HttpClient client;
		if (!useProxy)
		{
			client = new HttpClient();
		}
		else if (useProxy && proxy != null)
		{
			HttpClientHandler handl = new HttpClientHandler();
			handl.Proxy = proxy;
			handl.UseProxy = true;
			client = new HttpClient((HttpMessageHandler)(object)handl);
		}
		else
		{
			client = new HttpClient();
		}
		string number = await GetNumber();
		if (number != "ERROR")
		{
			string _id = number.Split(' ')[0];
			string _n = number.Split(' ')[1];
			HttpRequestMessage val = new HttpRequestMessage
			{
				RequestUri = new Uri(PHONE_URL)
			};
			((HttpHeaders)val.Headers).Add("Authorization", token ?? "");
			val.Content = (HttpContent)new StringContent("{\"phone\": \"+" + _n + "\"}", Encoding.UTF8, "application/json");
			val.Method = HttpMethod.Post;
			HttpRequestMessage req_sub = val;
			await client.SendAsync(req_sub);
			bool ok_ = false;
			await Task.Run(async delegate
			{
				bool res = false;
				while (!res)
				{
					Thread.Sleep(1300);
					string smc = await (await client.GetAsync(GET_NUMB + "?api_key=" + _token + "&action=getStatus&id=" + _id)).Content.ReadAsStringAsync();
					if (!(smc == "STATUS_WAIT_CODE"))
					{
						if (smc.Contains("STATUS_OK"))
						{
							string _code = smc.Split(':')[1];
							HttpRequestMessage val2 = new HttpRequestMessage
							{
								RequestUri = new Uri(VERIFY_URL)
							};
							((HttpHeaders)val2.Headers).Add("Authorization", token ?? "");
							val2.Content = (HttpContent)new StringContent("{\"code\": " + _code.Replace(" ", "") + "}", Encoding.UTF8, "application/json");
							val2.Method = HttpMethod.Post;
							HttpRequestMessage req_verify = val2;
							if (await (await client.SendAsync(req_verify)).Content.ReadAsStringAsync() == "")
							{
								ok_ = true;
							}
							else
							{
								ok_ = false;
							}
						}
						break;
					}
					res = false;
				}
			});
			if (ok_)
			{
				return true;
			}
			return false;
		}
		return false;
	}
}
