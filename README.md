# Xamarin Weather App
![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)

**Weather App** is a simple weather app developed with Xamarin. The app allows you to see the weather from your current location or another location around the globe using **OpenWeather Api**. Using **One Call API** the app display the current weather, 24 hours and 6 days forward forecast. You can add and switch location from Locations screen and also you can switch unit from metric to imperial. The app is available in *light mode* and *dark mode*.

## Screenshots

![Welcome](https://github.com/[username]/[reponame]/blob/[branch]/welcome.png?raw=true)

#### Light mode

![Weather](https://github.com/[username]/[reponame]/blob/[branch]/light_weather.png?raw=true)
![SideMenu](https://github.com/[username]/[reponame]/blob/[branch]/light_sidemenu.png?raw=true)
![Locations](https://github.com/[username]/[reponame]/blob/[branch]/light_locations.png?raw=true)
![AddLocation](https://github.com/[username]/[reponame]/blob/[branch]/light_addlocation.png?raw=true)
![Settings](https://github.com/[username]/[reponame]/blob/[branch]/light_settings.png?raw=true)

#### Dark mode
![Weather](https://github.com/[username]/[reponame]/blob/[branch]/dark_weather.png?raw=true)
![SideMenu](https://github.com/[username]/[reponame]/blob/[branch]/dark_sidemenu.png?raw=true)
![Locations](https://github.com/[username]/[reponame]/blob/[branch]/dark_locations.png?raw=true)
![AddLocation](https://github.com/[username]/[reponame]/blob/[branch]/dark_addlocation.png?raw=true)
![Settings](https://github.com/[username]/[reponame]/blob/[branch]/dark_settings.png?raw=true)

## Libraries
- [Xamarin Forms 5.0](https://github.com/xamarin/Xamarin.Forms)
- [Xamarin Essentials](https://github.com/xamarin/Essentials) (Location, Placemark, Internet Connection) 
- [Xamarin CommunityToolkit](https://github.com/xamarin/XamarinCommunityToolkit) (SideMenu, StateLayout, TouchEffect, Expander)
- [Prism.Forms](https://github.com/PrismLibrary/Prism) (MVVM, Dialogs)
- [Fody](https://github.com/Fody/Fody)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [Xamarin.Plugin.SharedTransitions](https://github.com/GiampaoloGabba/Xamarin.Plugin.SharedTransitions) (Page Transitions)

## Setup
The app is using **One Call API** from OpenWeather Api. To start the project you need an **account** and **OpenWeather Api Key**.

Create a file called **local.settings.json** in the root of the WeatherApp project.
```json
{
  "openWeatherMapApiBaseUrl": "https://api.openweathermap.org/data/2.5",
  "openWeatherMapApiKey": "YOUR_KEY"
}
```
For **local.settings.json** go to **Properties** and select **Embedded resource** from **Build Action**

## Resources
Illustrations are from [Freepik](https://www.freepik.com/)
