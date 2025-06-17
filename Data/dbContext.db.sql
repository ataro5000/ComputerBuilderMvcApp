BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "AspNetUserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "AspNetUserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "AspNetUsers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetUsers" PRIMARY KEY,
    "FirstName" TEXT NULL,
    "LastName" TEXT NULL,
    "Address" TEXT NULL,
    "City" TEXT NULL,
    "Province" TEXT NULL,
    "PostalCode" TEXT NULL,
    "Country" TEXT NULL,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "Email" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);
CREATE TABLE IF NOT EXISTS "Component" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Component" PRIMARY KEY AUTOINCREMENT,
    "Image" TEXT NULL,
    "Name" TEXT NULL,
    "PriceCents" TEXT NOT NULL,
    "Spec" TEXT NULL,
    "Type" TEXT NULL
);
CREATE TABLE IF NOT EXISTS "OrderItems" (
    "OrderItemId" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "OrderId" INTEGER NOT NULL,
    "ComponentId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "UnitPrice" decimal(18, 2) NOT NULL,
    CONSTRAINT "FK_OrderItems_Component_ComponentId" FOREIGN KEY ("ComponentId") REFERENCES "Component" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("OrderId") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Orders" (
    "OrderId" INTEGER NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "CustomerId" TEXT NOT NULL,
    "OrderDate" TEXT NOT NULL,
    "TotalAmount" decimal(18, 2) NOT NULL, "ShippingAddress" TEXT NOT NULL DEFAULT '', "Status" INTEGER NOT NULL DEFAULT 0,
    CONSTRAINT "FK_Orders_AspNetUsers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Reviews" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Reviews" PRIMARY KEY AUTOINCREMENT,
    "Comments" TEXT NULL,
    "CustomerName" TEXT NULL,
    "ItemId" INTEGER NOT NULL,
    "Rating" TEXT NOT NULL,
    "ReviewDate" TEXT NOT NULL,
    CONSTRAINT "FK_Reviews_Component_ItemId" FOREIGN KEY ("ItemId") REFERENCES "Component" ("Id") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS "__EFMigrationsLock" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK___EFMigrationsLock" PRIMARY KEY,
    "Timestamp" TEXT NOT NULL
);
INSERT INTO "AspNetUserTokens" VALUES ('eb35a1ba-e034-475f-adfd-d15ce7a11a9d','[AspNetUserStore]','AuthenticatorKey','ADOM2ZD3U5RHLABJZYFMRENWVGGNXIVF');
INSERT INTO "AspNetUsers" VALUES ('eb35a1ba-e034-475f-adfd-d15ce7a11a9d','shane','edwards',NULL,NULL,NULL,NULL,NULL,'ataro5000@gmail.com','ATARO5000@GMAIL.COM','ataro5000@gmail.com','ATARO5000@GMAIL.COM',1,'AQAAAAIAAYagAAAAEGuUDPlLrEeKd1Mzlvsa0GuJPy70cvbPeDlOcSnRU5oF1JGI2vEACgiFT7LZwhQLpw==','3VL3PYSLQZWI37OXSZRE5GVHSE27KJWA','07b9d632-22f7-48c1-8783-dc7c17d4813b','123456767',0,0,NULL,1,0);
INSERT INTO "AspNetUsers" VALUES ('6964fc17-ceb5-43d6-8f39-4654494c5f35',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'sedwards59@hotmail.com','SEDWARDS59@HOTMAIL.COM','sedwards59@hotmail.com','SEDWARDS59@HOTMAIL.COM',0,'AQAAAAIAAYagAAAAENwbrU9ePGhuBrsuHBrfO7PU1qtpaZNd/WYeWOKi5czttzPC6y7YZZHlrQAXvWfgoQ==','GZUEAFDO2QIHBKNJOJYPUYHND4SQTRJ4','c04bb291-22e0-457a-94ae-78d6bcc1b8e7',NULL,0,0,NULL,1,0);
INSERT INTO "AspNetUsers" VALUES ('fdf42b94-4eaf-4bfd-b416-355313d51993',NULL,NULL,NULL,NULL,NULL,NULL,NULL,'shane.edwards5000@gmail.com','SHANE.EDWARDS5000@GMAIL.COM','shane.edwards5000@gmail.com','SHANE.EDWARDS5000@GMAIL.COM',1,'AQAAAAIAAYagAAAAEA66QXpsaNq8/Q8wM6Ylud7HSqjc9Ib8UWoIa0VdJW8BKdcbiYIBk/MMwkGLfGBG0Q==','X3PLSDP36LGMCJI7IPZSCEBRHIXNTLON','58cd024d-b2c4-4b58-8a67-d5602c19bd66',NULL,0,0,NULL,1,0);
INSERT INTO "Component" VALUES (1,'Images/products/case1.jpg','Regular Joe','499.0','Who needs cable management?','Case');
INSERT INTO "Component" VALUES (2,'Images/products/case2.jpg','Office Workstation','4999.0','Room to play with and make everything look nice!','Case');
INSERT INTO "Component" VALUES (3,'Images/products/SSD.jpg','1TB SSD','4999.0','Read: 6000MB/s; Write:4000MB/s M.2','Storage');
INSERT INTO "Component" VALUES (4,'Images/products/SSD.jpg','2TB SSD','5999.0','Read: 6000MB/s; Write:4000MB/s M.2','Storage');
INSERT INTO "Component" VALUES (5,'Images/products/case3.jpg','Gaming Beast','49999.0','The monster truck of Cases, More RGB than Vegas!','Case');
INSERT INTO "Component" VALUES (6,'Images/products/CPU1.jpg','Intel Core i7-12700KF','49999.0','Upto 5 GHz 12 Cores','CPU');
INSERT INTO "Component" VALUES (7,'Images/products/CPU2.jpg','AMD Ryzen 9 9600X','54999.0','3.9 GHz 6 Cores','CPU');
INSERT INTO "Component" VALUES (8,'Images/products/CPU1.jpg','Intel Core i7-11700K','39999.0','3.6 GHz 8 Cores','CPU');
INSERT INTO "Component" VALUES (9,'Images/products/CPU2.jpg','AMD Ryzen 7 5800X','44999.0','3.8 GHz 8 Cores','CPU');
INSERT INTO "Component" VALUES (10,'Images/products/CPU1.jpg','Intel Core i5-11600K','26299.0','3.9 GHz 6 Cores','CPU');
INSERT INTO "Component" VALUES (11,'Images/products/NVGPU.jpg','NVIdIA GeForce RTX 3080','69999.0','10GB','GPU');
INSERT INTO "Component" VALUES (12,'Images/products/AMDGPU.jpg','AMD Radeon RX 6800 XT','64999.0','16GB','GPU');
INSERT INTO "Component" VALUES (13,'Images/products/NVGPU.jpg','NVIdIA GeForce RTX 3070','49999.0','8GB','GPU');
INSERT INTO "Component" VALUES (14,'Images/products/AMDGPU.jpg','AMD Radeon RX 6700 XT','47999.0','12GB','GPU');
INSERT INTO "Component" VALUES (15,'Images/products/NVGPU.jpg','NVIdIA GeForce GTX 1660 Super','24999.0','6GB','GPU');
INSERT INTO "Component" VALUES (16,'Images/products/MB.jpg','ASUS ROG Strix B550-F Gaming','19999.0','AM5 ATX','MotherBoard');
INSERT INTO "Component" VALUES (17,'Images/products/MB.jpg','MSI MPG B550 Gaming Plus','14999.0',' AM4 ATX','MotherBoard');
INSERT INTO "Component" VALUES (18,'Images/products/MB.jpg','Gigabyte B450 AORUS Elite','7999.0',' AM4 ATX','MotherBoard');
INSERT INTO "Component" VALUES (19,'Images/products/MB.jpg','ASRock B450M Pro4','7999.0','AM4','MotherBoard');
INSERT INTO "Component" VALUES (20,'Images/products/MB.jpg','ASUS TUF Gaming X570-Plus','18999.0','AM6','MotherBoard');
INSERT INTO "Component" VALUES (21,'Images/products/RAM.jpg','8GB DDR4','5999.0','3200MHz','RAM');
INSERT INTO "Component" VALUES (22,'Images/products/RAM.jpg','16GB DDR4','8999.0','3600MHz','RAM');
INSERT INTO "Component" VALUES (23,'Images/products/RAM.jpg','32GB DDR4','15999.0','3600MHz','RAM');
INSERT INTO "Component" VALUES (24,'Images/products/RAM.jpg','64GB DDR4','29999.0','3600MHz','RAM');
INSERT INTO "Component" VALUES (25,'Images/products/RAM.jpg','16GB DDR5','11999.0','6000MHz','RAM');
INSERT INTO "Component" VALUES (26,'Images/products/RAM.jpg','32GB DDR5','23999.0','6000MHz','RAM');
INSERT INTO "Component" VALUES (27,'Images/products/PSU.jpg','Corsair RM750x','12999.0','750 Watts','PSU');
INSERT INTO "Component" VALUES (28,'Images/products/PSU.jpg','EVGA 600 W1','4999.0','600 Watts','PSU');
INSERT INTO "Component" VALUES (29,'Images/products/PSU.jpg','Seasonic Focus GX-850','13999.0','850 Watts','PSU');
INSERT INTO "Component" VALUES (30,'Images/products/PSU.jpg','Thermaltake Toughpower GF1 750W','10999.0','750 Watts','PSU');
INSERT INTO "Component" VALUES (31,'Images/products/PSU.jpg','Cooler Master MWE Gold 650W','8999.0','650 Watts','PSU');
INSERT INTO "OrderItems" VALUES (1,1,31,1,89.99);
INSERT INTO "OrderItems" VALUES (2,2,15,1,249.99);
INSERT INTO "OrderItems" VALUES (3,3,2,1,49.99);
INSERT INTO "OrderItems" VALUES (4,3,1,1,4.99);
INSERT INTO "OrderItems" VALUES (5,3,5,1,499.99);
INSERT INTO "OrderItems" VALUES (6,4,15,1,249.99);
INSERT INTO "OrderItems" VALUES (7,4,21,1,59.99);
INSERT INTO "OrderItems" VALUES (8,4,16,1,199.99);
INSERT INTO "OrderItems" VALUES (9,5,2,1,49.99);
INSERT INTO "OrderItems" VALUES (10,5,1,1,4.99);
INSERT INTO "OrderItems" VALUES (11,5,5,1,499.99);
INSERT INTO "OrderItems" VALUES (12,5,7,3,549.99);
INSERT INTO "OrderItems" VALUES (13,5,9,1,449.99);
INSERT INTO "OrderItems" VALUES (14,6,20,1,189.99);
INSERT INTO "OrderItems" VALUES (15,7,20,1,189.99);
INSERT INTO "OrderItems" VALUES (16,7,12,1,649.99);
INSERT INTO "OrderItems" VALUES (17,8,2,2,49.99);
INSERT INTO "OrderItems" VALUES (18,9,25,2,119.99);
INSERT INTO "OrderItems" VALUES (19,9,2,1,49.99);
INSERT INTO "OrderItems" VALUES (20,10,9,1,449.99);
INSERT INTO "OrderItems" VALUES (21,10,11,1,699.99);
INSERT INTO "OrderItems" VALUES (22,11,8,1,399.99);
INSERT INTO "OrderItems" VALUES (23,11,12,1,649.99);
INSERT INTO "OrderItems" VALUES (24,12,27,1,129.99);
INSERT INTO "OrderItems" VALUES (25,13,10,4,262.99);
INSERT INTO "OrderItems" VALUES (26,14,2,3,49.99);
INSERT INTO "OrderItems" VALUES (27,14,7,1,549.99);
INSERT INTO "OrderItems" VALUES (28,14,9,1,449.99);
INSERT INTO "OrderItems" VALUES (29,14,11,1,699.99);
INSERT INTO "OrderItems" VALUES (30,14,4,1,59.99);
INSERT INTO "OrderItems" VALUES (31,14,13,1,499.99);
INSERT INTO "OrderItems" VALUES (32,15,4,1,59.99);
INSERT INTO "OrderItems" VALUES (33,15,21,1,59.99);
INSERT INTO "OrderItems" VALUES (34,16,2,3,49.99);
INSERT INTO "Orders" VALUES (1,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-12 14:04:47.4401044',89.99,'',4);
INSERT INTO "Orders" VALUES (2,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-12 16:16:33.1783844',249.99,'59 Rivercrest Lane',4);
INSERT INTO "Orders" VALUES (3,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-12 16:19:43.5562678',554.97,'59 Rivercrest Lane',4);
INSERT INTO "Orders" VALUES (4,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-12 17:48:47.8918044',509.97,'59 Rivercrest Lane',4);
INSERT INTO "Orders" VALUES (5,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-12 17:51:04.5904565',2654.93,'59 Rivercrest Lane',4);
INSERT INTO "Orders" VALUES (6,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-12 18:04:29.2743114',189.99,'59 Rivercrest Ln',4);
INSERT INTO "Orders" VALUES (7,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-13 16:28:08.5508018',839.98,'59 Rivercrest Ln',0);
INSERT INTO "Orders" VALUES (8,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-13 16:29:37.2693511',99.98,'59 Rivercrest Lane',0);
INSERT INTO "Orders" VALUES (9,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-13 16:30:56.6605472',289.97,'59 Rivercrest Lane',0);
INSERT INTO "Orders" VALUES (10,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-13 16:31:15.2939551',1149.98,'59 Rivercrest Lane',0);
INSERT INTO "Orders" VALUES (11,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-13 16:34:13.0499895',1049.98,'59 Rivercrest Ln',0);
INSERT INTO "Orders" VALUES (12,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-13 16:58:05.3289943',129.99,'59 Rivercrest Ln',4);
INSERT INTO "Orders" VALUES (13,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-13 23:27:21.7125668',1051.96,'59 Rivercrest Ln',4);
INSERT INTO "Orders" VALUES (14,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-14 00:40:51.4898852',2409.92,'59 Rivercrest Lane',0);
INSERT INTO "Orders" VALUES (15,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-14 20:56:44.7866959',119.98,'59 Rivercrest Ln',4);
INSERT INTO "Orders" VALUES (16,'eb35a1ba-e034-475f-adfd-d15ce7a11a9d','2025-06-16 11:06:57.135757',149.97,'59 Rivercrest Ln',0);
INSERT INTO "Reviews" VALUES (1,'So cool','Shane Edwards',8,'50.0','2025-06-11 13:11:33.6409171');
INSERT INTO "Reviews" VALUES (2,'123451','Shane Edwards',8,'50.0','2025-06-11 13:22:03.2629864');
INSERT INTO "Reviews" VALUES (3,'I came back again!','Shane Edwards',4,'50.0','2025-06-11 13:39:07.1964431');
INSERT INTO "Reviews" VALUES (4,'1234','1234',2,'40.0','2025-06-11 13:59:55.6337144');
INSERT INTO "Reviews" VALUES (5,'holy cow','Shane Edwards',4,'50.0','2025-06-11 14:17:37.3083816');
INSERT INTO "Reviews" VALUES (6,'asdfasdf','shane',1,'40.0','2025-06-11 14:41:04.2876902');
INSERT INTO "Reviews" VALUES (7,'not bad','1234',12,'30.0','2025-06-11 15:06:40.1202551');
INSERT INTO "Reviews" VALUES (8,'This is a review','Anonymous',15,'30.0','2025-06-11 15:47:11.6253126');
INSERT INTO "Reviews" VALUES (9,'asdfasf','shane',6,'50.0','2025-06-12 11:28:55.0201623');
INSERT INTO "Reviews" VALUES (10,'test','Shane Edwards',5,'40.0','2025-06-12 11:36:15.509106');
INSERT INTO "Reviews" VALUES (11,'test1','Shane Edwards',5,'50.0','2025-06-12 11:39:56.8495602');
INSERT INTO "Reviews" VALUES (12,'test ValidateNever','okay',16,'50.0','2025-06-12 13:00:59.9290601');
INSERT INTO "Reviews" VALUES (13,'testing again.','Shane Edwards',20,'50.0','2025-06-12 13:02:32.2790433');
INSERT INTO "Reviews" VALUES (14,'2331412','1234',20,'40.0','2025-06-12 14:23:15.7822853');
INSERT INTO "Reviews" VALUES (15,'asdfasdf','Shane Edwards',2,'0.0','2025-06-12 16:29:07.3849328');
INSERT INTO "Reviews" VALUES (16,'my review','Shane Edwards',10,'40.0','2025-06-13 17:27:42.4526086');
INSERT INTO "__EFMigrationsHistory" VALUES ('20250611183254_InitialSqliteSchema','9.0.6');
INSERT INTO "__EFMigrationsHistory" VALUES ('20250611185540_AddedAppEntities','9.0.6');
INSERT INTO "__EFMigrationsHistory" VALUES ('20250612140110_CreateOrderAndOrderItemTables','9.0.6');
INSERT INTO "__EFMigrationsHistory" VALUES ('20250612145533_AddShippingAddressToOrder','9.0.6');
INSERT INTO "__EFMigrationsHistory" VALUES ('20250612161059_AddShippingAddressToOrders','9.0.6');
INSERT INTO "__EFMigrationsHistory" VALUES ('20250612173031_AddOrderStatusToOrder','9.0.6');
INSERT INTO "__EFMigrationsLock" VALUES (1,'2025-06-13 17:31:38.8483301+00:00');
CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
CREATE INDEX "IX_OrderItems_ComponentId" ON "OrderItems" ("ComponentId");
CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");
CREATE INDEX "IX_Orders_CustomerId" ON "Orders" ("CustomerId");
CREATE INDEX "IX_Reviews_ItemId" ON "Reviews" ("ItemId");
CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");
COMMIT;
