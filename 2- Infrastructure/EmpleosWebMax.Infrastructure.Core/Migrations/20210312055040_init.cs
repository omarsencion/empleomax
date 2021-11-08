using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EmpleosWebMax.Infrastructure.Core.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeUser = table.Column<int>(type: "int", nullable: false),
                    Sexo = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeAdd = table.Column<int>(type: "int", nullable: false),
                    StatusGeneral = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "educacion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tipoestudio = table.Column<int>(type: "int", nullable: false),
                    Institucion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitucionLugar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    desde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    dateadd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_educacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "empleo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Job = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Idnumempresa = table.Column<long>(type: "bigint", nullable: false),
                    Idempresa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulotrabajo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripciontrabajo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requisitostrabajo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ciudadtrabajo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salariotratar = table.Column<bool>(type: "bit", nullable: false),
                    Salario = table.Column<double>(type: "float", nullable: false),
                    Salariohasta = table.Column<double>(type: "float", nullable: false),
                    publicosino = table.Column<bool>(type: "bit", nullable: false),
                    desde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoContrato = table.Column<int>(type: "int", nullable: false),
                    jornadahrs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    diaslaborables = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    edadminima = table.Column<int>(type: "int", nullable: false),
                    edadmaxima = table.Column<int>(type: "int", nullable: false),
                    sexo = table.Column<int>(type: "int", nullable: false),
                    idiomas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    dateadd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Areaprofesional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    salarioultimoMON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    salarioaspiraMON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    statusGral = table.Column<int>(type: "int", nullable: false),
                    statusGralBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    statusGralDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    statusGralMail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "empleoAdds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdJob = table.Column<long>(type: "bigint", nullable: false),
                    Job = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    vistapor = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    dateadd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tracking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailCandidato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TituloEmpleo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeAddEmpresa = table.Column<int>(type: "int", nullable: false),
                    statusSenMail = table.Column<int>(type: "int", nullable: false),
                    IdUserEmpresa = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleoAdds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "empleoAddTrackings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmppleoAdd = table.Column<long>(type: "bigint", nullable: false),
                    Tracking = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Job = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUserCandidato = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUserEmpresa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vistopor = table.Column<long>(type: "bigint", nullable: false),
                    Msg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tracking_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdReferencia = table.Column<long>(type: "bigint", nullable: false),
                    From_ = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    To_ = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusTracking = table.Column<int>(type: "int", nullable: false),
                    elCandidato = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleoAddTrackings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "empresaperfils",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Idempresa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresaCentro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    dateadd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresaperfils", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "experiencias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Posicion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuncionesRol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aportes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    desde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    dateadd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experiencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "follows",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUserPrincipal = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MailPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeUserPrincipal = table.Column<int>(type: "int", nullable: false),
                    IdUserEmpresa = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MailEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeUserEmpresa = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    fechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    seguidor = table.Column<int>(type: "int", nullable: false),
                    seguidorStatus = table.Column<int>(type: "int", nullable: false),
                    seguidorStatusFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    solicitudEnviada = table.Column<int>(type: "int", nullable: false),
                    solicitudRecibida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "foroCategorias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusForoCategoria = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foroCategorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForoLikes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPost = table.Column<long>(type: "bigint", nullable: false),
                    IdForo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Like = table.Column<short>(type: "smallint", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForoLikes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foromensajes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdForoInt = table.Column<long>(type: "bigint", nullable: false),
                    IdForo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUserPlataforma = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusForoUser = table.Column<short>(type: "smallint", nullable: false),
                    StatusForoAdmin = table.Column<short>(type: "smallint", nullable: false),
                    PublicadoMsg = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foromensajes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foros",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdForo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdCategoria = table.Column<short>(type: "smallint", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusForo = table.Column<short>(type: "smallint", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Publicado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "friendsall",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUserPrincipal = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MailPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeUserPrincipal = table.Column<int>(type: "int", nullable: false),
                    IdUserGuest = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MailGuest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeUserGuest = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    fechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amigo = table.Column<int>(type: "int", nullable: false),
                    amigoStatus = table.Column<int>(type: "int", nullable: false),
                    amigoStatusFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    seguidor = table.Column<int>(type: "int", nullable: false),
                    seguidorStatus = table.Column<int>(type: "int", nullable: false),
                    seguidorStatusFecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    solicitudEnviada = table.Column<int>(type: "int", nullable: false),
                    solicitudRecibida = table.Column<int>(type: "int", nullable: false),
                    mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nameinvitado = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friendsall", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "miscelaneos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_miscelaneos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "referencias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Persona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parentezco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    dateadd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_referencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketResponses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Respuesta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From_ = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    To_ = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusResponse = table.Column<short>(type: "smallint", nullable: false),
                    FechaResponse = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From_ = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    To_ = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusTicket = table.Column<short>(type: "smallint", nullable: false),
                    FechaTicket = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDocs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocNameTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Estadocivil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estadolaboral = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefono2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nacionalidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ultimosalario = table.Column<double>(type: "float", nullable: false),
                    Salarioaspira = table.Column<double>(type: "float", nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Areaprofesional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CVconfidencial = table.Column<bool>(type: "bit", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateadd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    salarioultimoMON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    salarioaspiraMON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentoIDn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentoIDt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idioma1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idioma2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idioma3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Idioma1nivel = table.Column<int>(type: "int", nullable: false),
                    Idioma2nivel = table.Column<int>(type: "int", nullable: false),
                    Idioma3nivel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "educacion");

            migrationBuilder.DropTable(
                name: "empleo");

            migrationBuilder.DropTable(
                name: "empleoAdds");

            migrationBuilder.DropTable(
                name: "empleoAddTrackings");

            migrationBuilder.DropTable(
                name: "empresaperfils");

            migrationBuilder.DropTable(
                name: "experiencias");

            migrationBuilder.DropTable(
                name: "follows");

            migrationBuilder.DropTable(
                name: "foroCategorias");

            migrationBuilder.DropTable(
                name: "ForoLikes");

            migrationBuilder.DropTable(
                name: "Foromensajes");

            migrationBuilder.DropTable(
                name: "Foros");

            migrationBuilder.DropTable(
                name: "friendsall");

            migrationBuilder.DropTable(
                name: "miscelaneos");

            migrationBuilder.DropTable(
                name: "referencias");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "TicketResponses");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "UserDocs");

            migrationBuilder.DropTable(
                name: "userInfos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
