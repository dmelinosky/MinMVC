#### Dependency Injection

##### Helpful Links

[How to on Stack Overflow](https://stackoverflow.com/questions/43311099/how-to-create-dependency-injection-for-asp-net-mvc-5)


#### Configuration

##### Helpful Links

[Original inspiration from Docker on windows book](https://github.com/sixeyed/docker-on-windows/blob/master/ch05/src/NerdDinner.Core/Config.cs)

[New in ASP.Net 4.7.1](https://fluentbytes.com/override-classic-asp-net-web-config-configuration-settings-when-using-docker-containers/)

[User Secrets in 4.7](https://stackoverflow.com/questions/59536717/user-secrets-in-net-4-7-connectionstrings-format)

[Net Core in Legacy Projects](https://benfoster.io/blog/net-core-configuration-legacy-projects)

[MSDN Docs - Configuration Builders](https://docs.microsoft.com/en-us/aspnet/config-builder)

#### Trouble along the way

##### Problems and Solutions

CS0012: The type 'System.Object' is defined in an assembly that is not referenced. You must add a reference to assembly 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'.

[Solution](https://stackoverflow.com/questions/49925484/you-must-add-a-reference-to-assembly-netstandard-version-2-0-0-0)

#### New cool things I learned along the way

*web.config binding redirects*

- Remove all the dependantAssembly nodes from the web.config file.
- Rebuild the application
- All warnings about required binding redirects will show up in output
- **Switch to the Error List view**
- Double click the warning item referencing the required redirects.
- Answer Yes to the modal about adding the required redirects. 
- Rebuild and all warnings are gone.