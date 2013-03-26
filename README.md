#Team Rynkeby

###Charity hack project

##Security
We're using [Fluent Security](http://www.fluentsecurity.net/) so all access policies are declared in the [SecurityConfiguration](https://github.com/testinspiration/TeamRynkeby/blob/LastWorking/EventBooking/Security/SecurityConfiguration.cs) class.

##Settings
When adding appsettings you should name the key "Class:Prop", create a class in [Settings](https://github.com/testinspiration/TeamRynkeby/tree/LastWorking/EventBooking/Settings) and it will be populated via AutoFac

##Mail templates
The mail templates are stored in the database, using [NVelocity](https://nuget.org/packages/NVelocity/) as template engine.
For syntax information, read [this](http://velocity.apache.org/engine/releases/velocity-1.5/user-guide.html).

