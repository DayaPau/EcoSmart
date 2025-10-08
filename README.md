# ♻️ Eco-Smart — Plataforma MVC de Reciclaje Sostenible

> “Pequeñas acciones generan grandes cambios.” 🌱  
EkoTrack es una aplicación web desarrollada con **ASP.NET Core MVC** que promueve el reciclaje comunitario mediante un sistema de registro y seguimiento de materiales reciclados.  
Incluye autenticación segura con **BCrypt**, control de acceso por **roles** y un **CRUD completo** para registrar actividades de reciclaje.

---

## 🚀 **Objetivo del proyecto**

El propósito de EcoSmart es demostrar el uso del **patrón MVC** aplicando un caso práctico y real:  
un sistema donde los ciudadanos pueden registrar sus actividades de reciclaje y los centros pueden gestionar la información de manera segura.

Este proyecto fue creado como parte del curso de **Desarrollo de Aplicaciones Web con ASP.NET MVC**, con enfoque en **HTML, CSS, C# y seguridad**.

---

## 🧭 **Funcionalidades principales**

✅ Registro y login de usuarios con contraseñas encriptadas con **BCrypt**.  
✅ Autenticación con cookies y autorización por **roles**:
- 👤 **CIUDADANO:** CRUD de reciclaje (crear, leer, editar y eliminar registros).
- 🚛 **RECOLECTOR:** módulo de recolección.
- 🏢 **ADMIN_CENTRO:** gestión de centros de reciclaje.



<img width="1909" height="747" alt="image" src="https://github.com/user-attachments/assets/b99c23aa-9734-4dce-aeac-cdd43b778b13" />

✅ CRUD completo de reciclaje: tipo de material, cantidad, fecha y notas.  
✅ Filtros por material y rango de fechas.  

<img width="1913" height="751" alt="image" src="https://github.com/user-attachments/assets/62407069-047d-4e22-81fe-1236b8d887ce" />

✅ Interfaz responsive desarrollada con **Bootstrap 5**.  
✅ Validaciones automáticas con **Razor** y **Data Annotations**.


<img width="1897" height="734" alt="image" src="https://github.com/user-attachments/assets/ce0672f0-03d5-45d5-9cbd-2832905182bd" />



---

## 🧩 **Tecnologías utilizadas**

| Categoría | Tecnologías |
|------------|--------------|
| Backend | ASP.NET Core MVC (.NET 8), C# |
| Frontend | HTML5, CSS3, Razor, Bootstrap 5 |
| Seguridad | BCrypt.Net-Next |
| Base de datos | SQL Server + Entity Framework Core |
| Arquitectura | Patrón Modelo–Vista–Controlador (MVC) |

---

## 🔐 **Seguridad con BCrypt**

EkoTrack nunca guarda contraseñas en texto plano.  
Cada clave se **encripta con BCrypt**, un algoritmo que genera un hash único por usuario, con “sal” aleatoria.  
Esto garantiza que incluso si la base de datos fuera expuesta, las contraseñas seguirían protegidas.

```csharp
var hash = BCrypt.Net.BCrypt.HashPassword(password);
if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
{
    ModelState.AddModelError("", "Credenciales inválidas.");
}
