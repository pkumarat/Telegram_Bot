# UiPath.Telegram
Telegram Bot activities are used to connect to Telegram application from UiPath application.
In order to use these activities they must be placed or enclosed within "Telegram Connector Scope".

Pre-requisites - Telegram Bot must be created using BotFather and the user must have the Token in order to communicate with other groups or users.

## Activities
1. Send Message
2. Send Photo
3. Get Updates

## Send Message
In this activity Telegram Bot sends messages instantly to users or public group where Telegram Bot is also one of the users(of type admin).

### Input fields
- Chat ID - To accept the Chat ID of user or group. It's type is **Int64**.
  - *Example: Chat ID = 123456789 (private user) or Chat ID = -987654321*

- Text message - To accept the message or string to deliver to user or group. It's type is String.
  - *Example: Text message = "Welcome to the world of Bots!!!"*

## Send Photo
In this activity Telegram Bot sends Images or pictures instantly to users or public group.

### Input fields
- Chat ID - To accept the Chat ID of user or group. It's type is **Int64**.
  - *Example: Chat ID = 123456789 (private user) or Chat ID = -987654321*

- Image Path - To accept the physical location in computer where there the image exists. It's type is **String**.
  - *Example: Image Path = "D:\Images_Folder\test_image.png"*

- Image Text - To accept a description for the attached image. When no value is given, by default value "Image sent from Bot" is given. It's type is **String**.
  - *Example: Image Text = "Nice picture!"*

## Get Updates
In this activity Telegram Bot receives the text messages sent by the private user and users in group.

### Output fields
- Message List - Gives output of text messages in a list of strings. It's type is **List**<**String**>.
  - *Example: Message List = Msg_array (Variable Of type List of String)*
  
