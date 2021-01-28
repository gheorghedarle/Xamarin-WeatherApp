# Xamarin Weather App
![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)

**Weather App** is a simple weather app developed with Xamarin. The app allows you to see the weather from your current location or another location around the globe using **OpenWeather Api**. Using **One Call API** the app display the current weather, 24 hours and 6 days forward forecast. You can add and switch location from Locations screen and also you can switch unit from metric to imperial. The app is available in *light mode* and *dark mode*.

## Screenshots

#### Light mode

#### Dark mode

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
