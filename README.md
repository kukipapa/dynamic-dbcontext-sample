# dynamic-dbcontext-sample
This is a sample architecture using custom db connection string values to generate DbContext connections dynamically.

On startup 3 dbs are created, based on configuration in **appsettings.json**:

 - application db
 - 2 sample external db
 
 Also 2 sample users are created:
 
 - alphadb@mail.com
 - betadb@mail.com
 
 Password: **P@ssw0rd**
 
 After launching you can log in with one of the created users and see the query result from the external db associated to the user.
 
 External db connection string is stored in **[ExternalDbConnectionString]** field.
