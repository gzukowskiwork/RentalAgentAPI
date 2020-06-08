# flatMate

Update </br>
Added Swagger for API testing, instead of Postman. You can find it in project default address:</br>
localhost:5000/

Update
Id is integer

In solution there is folder Additional files. There You can find:</br>
Db_schema.png- schema for database, from MySql Workbench,</br>
FlatOwner.sql- script file for creating database, created in MySql Workbench,</br>
populateDb.txt- sample commands to populate database.</br>
</br>
To connect with database edit appsetting.json file.</br>
<code>"connection": {
    "connectionString": "server=localhost;userid=root;password=password;database=flatowner;"
  }
</code></br>
Edit userid and password to match Yours credentials. </br>
Script creates db thats named "flatowner", so if chcanged, also change database name.  </br>
</br>
Id is Guid, can be changed in future to int.

