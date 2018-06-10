# SunsGoneLightsOn
A .NET Core application that can be used with the TPLink series of remote plugs to link their power on cycles to when the sun sets. 

## Process
On initialization, the application will connect to the TPLink API service and enumerate all the plugs on the account. Immediately thereafter, it will calculate the time to the next sunet (as specified by the settings.json file). 

Upon discovering all associated information, the process enters a thread sleep until the calculated time. At which point, the remote plug will be triggered. 

After the plug is triggered, the process repeats.

## Setup
On first run, the application will begin to create a `settings.json` file in the root of the application folder. It will ask a few questions along with the latitude and longitude of your desired location. 

At the end of setup, you may choose to exit the application entirely or to keep it running with the specified settings.

You may create this file by hand by making a similar file using `Settings.schema.jsonc` as found in the root directory of this project.

## Prior Work(s)

### Alexandre Dumont's [itnerd.space](http://itnerd.space) Website
A large majority of the TPLink API was gleaned from the works of Alexandre Dumont via his blog post [here](http://itnerd.space/2017/01/22/how-to-control-your-tp-link-hs100-smartplug-from-internet/). 

#### Differences of note: 
 * This operates through C# mechanisms whereas the article works through `curl` requests. (Later articles indicate a JS library that can also perform similar behaviors)
 * This implementation only turns devices on at sunset. His will let you do anything you want whenever

### IFTTT Applets
 IFTTT does allow for similar integrations by linking WunderGround to Kasa. The applet works great, however I find there is often a full hour skew between sunset and applet activation. 

#### Differences of note:
 * This should fire on time (or earlier if specified via skewing)
 * (Potential to) fire multiple devices with one application
 * Currently this implementation can't email you if something goes wrong. It will just bail out


