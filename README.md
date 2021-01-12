# Fischer.WebAPI.AspCoreSolution
This POC was created to demonstrate a Web API Solution with 2 custom data layers for Entity Framework and ADO. The ADO Data Library uses SQL String queries that could have been
converted to Stored Procedures and the code augmented to use those. The Entity Framework Data Library was created afterward as an exercise on how to work with it. You will notice
the Web API code has regions for both SQL Testing and EF Testing. In a production scenario, this would not be there. Also, the connection string shown is only for testing and is 
from a local SQL Server on a VM. In a production setting it would be advisable to either encrypt the connection string or put it in Azure KeyVault/Connection settings.
**Note: this code was created by me for personal interest, apart from any IT projects, and should not be considered production-ready.
