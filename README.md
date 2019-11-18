# Connecting OrgChart to Active Directory

This article attempts to show you how to connect OrgChart to active directory using C# in a simple way. I made this simple web application using ASP.NET Core. The aaplication performs simple operations as showing all users in particular OU, disabling a user and reseting a password.

You should have some basic knowledge of ASP.NET MVC.

In my case, I use test Active Directory system and run the code on the domain controler.

There are two namespaces to communicate Active Directory with C#:
**System.DirectoryServices.ActiveDirectory** and
**System.DirectoryServices.AccountManagement** (this is what I used)
You need ot install it using the command shown here: 
https://www.nuget.org/packages/System.DirectoryServices.AccountManagement/4.7.0-preview2.19523.17

- Add Action called **OrgChart** in your Controller:
```
public ActionResult OrgChart()
{
    List<User> ADUsers = GetallAdUsers();
    return View(ADUsers);
}
```
