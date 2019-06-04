A tool to make typing out passwords on Android devices without a touchscreen like the Oculus Quest faster by using your Windows PC keyboard to type over ADB.

![Image of interface](https://github.com/wakearray/QuestTyperTool/blob/master/QTT.png)

Based on the answer to this question: https://stackoverflow.com/questions/56386237/is-it-possible-to-copy-paste-type-eg-a-password-from-an-app-into-the-oculus-q/56386580#56386580

First make sure you have ADB downloaded to your computer and you know where it is. If you don't have it, you can find it here: https://developer.android.com/studio/releases/platform-tools.html

This app will only work if ADB is either added to the system path or if the app is inside your ADB folder.

For an Oculus Quest or Go:

Create/join an organisation in the Oculus Dashboard
Open the Oculus app on your mobile phone.
In the Settings menu, select the Oculus Quest headset that you’re using for development.
Select More Settings.
Toggle Developer Mode on.
Once developer mode has been turned on you'll need to connect your headset to your PC and type something. The app will run the 'adb devices' command and you will need to authorize the PC using the prompt inside the headset. Once that's done, simply select the field you want typed into and put the text into the large text field of the app's interface and either push submit or hit enter.

When you've successfully sent the word test, it'll look like this:
![Image of interface](https://github.com/wakearray/QuestTyperTool/blob/master/QTT2.png)
