<img src="https://github.com/gheorghedarle/Xamarin-WeatherApp/blob/main/Screenshots/icon.png" width="96" /> 

# Xamarin Weather App

![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)

**Weather App** is a simple weather app developed using Xamarin Forms. The app allows you to see the weather from your current location or another location around the globe using **OpenWeather Api**. Using **One Call API** the app displays the current weather, 24 hours and 6 days forecast. You can add new locations and switch between them from the Locations screen. From Settings screen you can change the unit system from metric to imperial. The app is available in **light** and **dark mode**.

If you like this repository you can support me on

<a href="https://www.buymeacoffee.com/gheorghedarle" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/guidelines/download-assets-sm-1.svg" alt="Buy Me A Coffee" width="175"></a>

## Screenshots

#### Light mode
<img src="https://github.com/gheorghedarle/Xamarin-WeatherApp/blob/main/Screenshots/light_mode.png?raw=true" Width="1620" />

#### Dark mode
<img src="https://github.com/gheorghedarle/Xamarin-WeatherApp/blob/main/Screenshots/dark_mode.png?raw=true" Width="1620" />

## Libraries
- [Xamarin Forms 5.0](https://github.com/xamarin/Xamarin.Forms)
- [Xamarin Essentials](https://github.com/xamarin/Essentials) (Location, Placemark, Internet Connection) 
- [Xamarin CommunityToolkit](https://github.com/xamarin/XamarinCommunityToolkit) (SideMenu, StateLayout, TouchEffect, Expander)
- [Prism.Forms](https://github.com/PrismLibrary/Prism) (MVVM, Dialogs)
- [Fody](https://github.com/Fody/Fody)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

## Setup
The app is using **One Call API** from OpenWeather Api. To start the project you need an **account** and **OpenWeather Api Key**.

Create a file called **local.settings.json** in the root of the WeatherApp project. Add the following code in the file.
```json
{
  "openWeatherMapApiBaseUrl": "https://api.openweathermap.org/data/2.5",
  "openWeatherMapApiKey": "YOUR_KEY"
}
```
For **local.settings.json** go to **Properties** and select **Embedded resource** from **Build Action**

## Resources
Illustrations are from [Freepik](https://www.freepik.com/)
