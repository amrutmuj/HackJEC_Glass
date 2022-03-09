using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class WeatherData : MonoBehaviour {
	private float timer;
	public float minutesBetweenUpdate;
	public WeatherDataInfo Info;
	public string API_key;
	public InputField Place;
	private float latitude;
	private float longitude;
	private bool locationInitialized;
	public Text currentWeatherText;
	public GetLocation getLocation;

	public void Begin() {
		latitude = getLocation.latitude;
		longitude = getLocation.longitude;
		locationInitialized = true;
		Place = GetComponent<InputField>();
	}
	void Update() {
		if (locationInitialized) {
			if (timer <= 0) {
				StartCoroutine (GetWeatherInfo ());
				timer = minutesBetweenUpdate * 60;
			} else {
				timer -= Time.deltaTime;
			}
		}
	}
	private IEnumerator GetWeatherInfo()
	{
		var www = new UnityWebRequest("https://api.weatherapi.com/v1/forecast.json?key=" + API_key + "&q=" + Place.text.ToString() + "&days=1&aqi=no&alerts=no")
		{
			downloadHandler = new DownloadHandlerBuffer()
		};

		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			//error
			yield break;
		}

		Info = JsonUtility.FromJson<WeatherDataInfo>(www.downloadHandler.text);
		currentWeatherText.text = "Current weather: " + Info.condition.text;
	}
}


[Serializable]
public class WeatherDataInfo
{
	public Location location;
	public Current current;
	public Condition condition;
}

[Serializable]
public class Location
{
	public string name;
	public string region;
	public string country;
	public string lat;
	public string lon;
}

[Serializable]
public class Current
{
	public double temp_c;
	public Condition current;
}

[Serializable]
public class Condition
{
	public string text;
	public string icon;
}






[Serializable]
public class WeatherInfo
{
	public float latitude;
	public float longitude;
	public string timezone;
	public Currently currently;
	public int offset;
}

[Serializable]
public class Currently
{
	public int time;
	public string summary;
	public string icon;
	public int nearestStormDistance;
	public int nearestStormBearing;
	public int precipIntensity;
	public int precipProbability;
	public double temperature;
	public double apparentTemperature;
	public double dewPoint;
	public double humidity;
	public double pressure;
	public double windSpeed;
	public double windGust;
	public int windBearing;
	public int cloudCover;
	public int uvIndex;
	public double visibility;
	public double ozone;
}
