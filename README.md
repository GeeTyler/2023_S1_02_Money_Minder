
<hr>

<div id="user-content-toc">
  <ul>
    <summary><h1 style="display: inline-block;">2023_S1_02_Money_Minder</h1></summary>
  </ul>
</div>

<hr>

### Table of Contents:
- [Description](#description)<br>
  - [Overview](#overview)<br>
  - [Key Features](#key-features)<br>
- [Installation](#installation)<br>
  - [Prerequisites](#prerequisites)
  - [Installation Steps](installation-steps)
- [Usage](#usage)<br>
  - [Configuration settings](#configuration-settings)
  - [Using the Program](#using-the-program)
- [Pages](#pages)<br>
- [Contributors](#contributors)<br>
- [Credits](#credits)<br>

<hr>

### Description:

#### Overview:

In today's complex and interconnected world, managing personal finances can be a daunting task. With numerous financial applications available, 
individuals often find it challenging to keep track of their purchases, investments, and overall financial health. Recognizing this problem, 
our team set out to develop a comprehensive web application that consolidates all these functions into a single, user-friendly platform. Our 
primary objective was to simplify the process of managing finances for individuals. By integrating various financial functions into a single 
web application, we aimed to streamline the entire process and eliminate the need for users to navigate multiple platforms.

#### Key Features:

• Be able to view account balances, make transfers in between user accounts<br>
• Be able to view the current stock market.<br>
• Market price information on each NZX company will be provided.<br>
• Bright and easy to read UI.

<hr>

### Installation:

#### Prerequisites:

• A device that can run windows<br>
• The latest version of Visual Studio<br>
• The .NET SDK<br>

#### Installation Steps:

Step 1:<br><br>

Download the latest version of Visual Studio from the offical website: https://visualstudio.microsoft.com/.<br><br>

Step 2:<br><br>

Now you can add the ASP.NET and web development workload to your Visual Studio setup:<br><br>

• Select the Windows key, type Visual Studio Installer, and press Enter.<br>
• If prompted, allow the installer to update itself.<br>
• If an update for Visual Studio 2022 is available, an Update button will be shown. Select it to update before modifying the installation.<br>
• Find your Visual Studio 2022 installation and select the Modify button.<br>
• If not selected already, select the ASP.NET and web development workload and select the Modify button. Otherwise, just close the dialog 
window.<br><br>

Step 3:<br><br>

Now you can clone this repository in your Visual Studio editor or you can just download it to your local machine and run it that way.<br><br>

Step 4:<br><br>

Be sure to open this program in Visual Studio on a Windows device.<br><br>

Before running this application make sure to:<br><br>
- If the migration folder exists, delete the migrations folder<br>
- If the moneyminder.db exists, delete the .db file as well<br>
- Open the Nuget package manager console in Visual Studio<br>
- Type in the following two commands:<br><br>
  - add-migration Initial
  - update-database<br><br>

Step 5:<br><br>
Now press the play button in your Visual Studio IDE and the program should start up.<br><br>

Facing any errors or difficulties:<br><br>

Visit this website for a comprehensive guide on running the program: https://dotnet.microsoft.com/en-us/learn/aspnet/blazor-tutorial/install

<hr>

### Usage:

#### Using the Program:

Once the program loads up, you should be on a signin page. There should be 2 buttons: a login button and a registration button. Click on the 
registration button to register your account. Once you are register you can go and log in to your account. Once you log in you should be on the 
main home page of the program. You should now be able to see all the pages on the left hand side navigation bar and interact with the program. All the pages should be self-explanatory.

<hr>

### Pages:

<br>AccountDetail.razor:

A page containing all transaction history of a particular account.<br><br>

BankAccounts.razor:

A page containing all the bank accounts linked to a particular account.<br><br>

Login.razor:

A page containing the login system for this program.<br><br>

Profile.razor:

A page containing the personal information of the user, allowing them to change particular details (name, email, password), delete profile.<br><br>
As well as registering new bank accounts, changing bank account names, and deleting bank accounts.<br><br>

Signup.razor:

A page containing the signup system for this program.<br><br>

StockMarket.razor:

A page containing all the different companies on the stock market.<br><br>

Company.razor:

A page containing historical market price data for a particular company.<br><br>

Transfer.razor:

A page containing the money transfer system for the program.<br>

<hr>

### Contributors:

• Arzan Mohta<br>(Development Team)<br><br>
• Dmitry Kirov<br>(Development Team)<br><br>
• Michael Moon<br>(Scrum Master)<br><br>
• Shreyas Singh<br>(Development Team)<br><br>
• Tyler Gee<br>(Product Owner)

<hr>

### Credits:

We used the blazor framework for this project. Blazor is a free and open-source web framework that enables developers to create web apps using 
C# and HTML. It is being developed by Microsoft.

<hr>
