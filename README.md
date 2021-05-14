# DemoProject
# Demo Porject API MVC

Este proyecto es una demostracion de una API Web con autenticacion JWT basada en Roles.

## Instalación

Restauramos todas las dependecians con NuGet Package Manager.
Y nos asguramos de Actualizar la Base de datos a las ultimas Migraciones.
```sh
Update-Database
```
Esto creará una Base de Datos local que por defecto esta prepara para una conexión locla de MSSQL, ademas de insertara un usario incial:
> UserName: Admin
> Password: Prueba1234$

Al cual se le asigna un rol incial de Administradoe tambien

## Funcionamiento
La API esta documentada con swagger y esta protegida con JWT.
Para conseguir el token se ha creado un formulario en la pagina de "home" que nos pedirá usuario y contraseña.
Una vez conseguido el Token si nos dirigimos a swagger:
> localhost:port/swagger

Y en el campo API_Key escribimos:
> Bearer {JWT Token}
>*no se aprecia pero hay un espacio entre Bearer y Token

Y con esto tendremos dos tipos de acceso.
`Admin`: Que nos dara accesso a toda la API.
`User`: El cual solo tendrá acceso a el Controllador de Usuario.

## Fundamento
Se puede observar que para esta demo (por un tema de facilitar la obtencion del JWT) la API actua de su propio "ResourceServer" Firmando y emitiendo sus propios JWT.
Los valores estas guardasdos en APPSETTINGS:
`Issuer`, `Secret`, `Audience` y `Expires`

Tambien se puede ver que asi como todo lo relacionado con identitdad y usuario se ha utilizado EF y MSSQL. En cambio, para la audiuecia se ha realizado un AudienceStore para guardarlo en tiempo de ejecución y que fuese más facil. Ya que en este ejemplo solo era un recurso para crear el JWT.
