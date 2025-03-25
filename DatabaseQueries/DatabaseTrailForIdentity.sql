use ClinicDb

SELECT * FROM AspNetUsers;
SELECT * FROM AspNetUsers WHERE Email = 'admin@example.com';

SELECT u.Email, r.Name AS RoleName
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'admin@example.com';

