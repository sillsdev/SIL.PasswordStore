// See https://aka.ms/new-console-template for more information

using SIL.Secrets;

const string user = "user";
const string service = "service";

PasswordStore.SetPassword(service, user, "assword");

var password = PasswordStore.GetPassword(service, user);
Console.WriteLine($"Got password: {password}");

PasswordStore.SetPassword(service, user, "NewPassword");
password = PasswordStore.GetPassword(service, user);
Console.WriteLine($"Changed password: {password}");

PasswordStore.DeletePassword(service, user);
password = PasswordStore.GetPassword(service, user);
Console.WriteLine($"After removing password: {password}");
