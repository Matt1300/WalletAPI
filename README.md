# Documentación del Proyecto WalletAPI

## Descripción General

WalletAPI es una API RESTful desarrollada en .NET 8 para la gestión de billeteras digitales y sus movimientos (transferencias). El proyecto sigue los principios de Clean Architecture y utiliza patrones y librerías modernas para garantizar mantenibilidad, escalabilidad y robustez.

---

## Endpoints Principales

### Billeteras

- `POST /api/wallets`

  - **Crear billetera**
  - Request: `{ "documentId": "string", "name": "string" }`
  - Response: `{ "id": int, "message": "string" }`

- `GET /api/wallets/{id}`

  - **Obtener billetera por ID**
  - Response: `{ "message": "string", "value": { ...datos de la billetera... } }`

- `PUT /api/wallets/{id}`

  - **Actualizar billetera**
  - Request: `{ "documentId": "string", "name": "string", "balance": decimal }`
  - Response: `{ "message": "string" }`

- `DELETE /api/wallets/{id}`
  - **Eliminar billetera**
  - Response: `{ "message": "string" }`

### Movimientos

- `POST /api/movements/transfer`

  - **Transferir saldo entre billeteras**
  - Request: `{ "sourceWalletId": int, "destinationWalletId": int, "amount": decimal }`
  - Response: `{ "message": "string" }`

- `GET /api/movements`
  - **Obtener todas las transferencias**
  - Response: `[{ ...datos de la transferencia... }]`

---

## Guía de Uso y Levantamiento

1. **Clona el repositorio.**
2. **Configura la conexión a la base de datos:**
   - Abre el archivo `appsettings.json` en el proyecto API.
   - Modifica el valor de `"DefaultConnection"` en la sección `"ConnectionStrings"` con tu cadena de conexión de SQL Server.
3. **Aplica migraciones:**
   - Ejecuta `dotnet ef database update` en la carpeta del proyecto API para crear el esquema de la base de datos.
4. **Levanta la API:**
   - Usa Visual Studio (**Start Debugging**) o ejecuta `dotnet run` en la carpeta del proyecto API.
   - En modo desarrollo, la documentación Swagger estará disponible en `/swagger` para probar los endpoints.

> **Importante:** La API no funcionará sin una cadena de conexión válida. Asegúrate de configurar `"DefaultConnection"` en tu archivo de configuración antes de ejecutar el proyecto.

---

## Justificación de Decisiones Técnicas

### Clean Architecture

- **Justificación:** Fundamental para proyectos a largo plazo. Permite una clara separación de preocupaciones, facilita el desarrollo paralelo, las pruebas y la evolución del sistema sin acoplamiento fuerte a tecnologías específicas. Mantiene la lógica de negocio central aislada y agnóstica.

### CQRS (Command Query Responsibility Segregation)

- **Justificación:** Mejora la claridad y el diseño. Separar comandos (escritura) de queries (lectura) simplifica los modelos, optimiza el rendimiento y facilita la aplicación de diferentes lógicas de validación y autorización. Se optó por una implementación manual de los handlers (`ICommandHandler`, `IQueryHandler`) sin MediatR para demostrar comprensión del patrón, por una mejor optimizacion y porque ahora MediatR pasa a ser comercial.

### Result Pattern

- **Justificación:** Es crucial para un manejo de errores robusto y explícito. Permite que los handlers devuelvan un objeto Result que indica claramente si una operación fue exitosa o falló, encapsulando tanto el valor de éxito como los mensajes de error asociados. Esto elimina la necesidad de lanzar excepciones para flujos de error esperados y mejora la legibilidad del código.

### FluentValidation

- **Justificación:** Permite definir reglas de validación complejas y legibles fuera de las entidades del dominio o los controladores. Se integra con un `ValidationBehavior` (Decorator) en el pipeline de CQRS, asegurando que las validaciones se ejecuten automáticamente antes de la lógica de negocio. Esto reduce la duplicación de código y mantiene la validación cerca de la definición de los comandos/queries.

### Repository Pattern y Unit of Work

- **Justificación:** Proporcionan una abstracción sobre la capa de persistencia. Los repositorios encapsulan la lógica de acceso a datos, mientras que el Unit of Work asegura que todas las operaciones dentro de una transacción se confirmen o reviertan juntas. Esto facilita el mocking para pruebas unitarias y permite cambiar la tecnología de base de datos en el futuro con menos impacto.

### Decorator Pattern (para Validación)

- **Justificación:** Se aplicó al implementar el `ValidationBehavior` que "envuelve" a los handlers de CQRS. Este patrón permite añadir funcionalidad (validación) a los handlers sin modificar su código base, manteniendo el principio de Abierto/Cerrado (SOLID).

### SQL Server con Entity Framework Core

- **Justificación:** Stack de persistencia robusto y común en el ecosistema .NET. EF Core proporciona un ORM que simplifica la interacción con la base de datos, manejando el mapeo objeto-relacional y las operaciones CRUD básicas.

---

Se dejaron como opcionales o se simplificaron al máximo otras funcionalidades (ej., Autenticación/Autorización, Cache) para cumplir con la restricción de tiempo, enfocándose en la entrega de una solución funcional y bien diseñada en el núcleo.

---

Para más detalles, revisa el código fuente y la documentación Swagger generada en tiempo de ejecución.
