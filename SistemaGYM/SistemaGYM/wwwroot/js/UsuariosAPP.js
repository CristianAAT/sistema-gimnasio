

class UsuariosAPP {

    constructor() {
        this.Id = "hola";
    }

    UsuariosAPP() {
    }

    Filtrar(Pagina, Value) {
        Value = (Value == "") ? "null" : Value;
        let Registros = $("#RegsPerPag").val();

        $.post(
            "Filtrar",
            { Pagina, Registros, Value },
            (response) => {
                $("#ResultTable").html(response[2]);
                $("#NavegadorTable").html(response[1]);
                $("#InfoTable").html(response[0]);
            }

        );
    }

    AgregarUsuario() {
        document.getElementById("mensaje").innerHTML = "";
        let Data = $(".Agregar").serialize();
        let Rol = $('#Rol').find(":selected").val();

        if (Rol == "null")
            Rol = "Miembro";

        Data = Data + "&Rol=" + Rol;
        

        $.post(
            "Agregar",
            Data,
            (response) => {
                try {
                    var Respuesta = JSON.parse(response);
                    if (Respuesta.Code == "OK") {
                        console.log(Respuesta);
                        $('#mAgregar').modal('hide');
                        this.ClearModal();
                        this.Filtrar(1, '');
                        this.Alertas(1);
                    } else {
                        console.log(Respuesta);
                        document.getElementById("mensaje").innerHTML = Respuesta.Description;


                    }
                } catch (ex) {
                    console.log(ex);
                }

            }
        );
    }

    EliminarUsuario() {
        console.log(this.Id);
        $.ajax({
            url: "Eliminar",
            type: "POST",
            data: { "Id" : this.Id },
            success: (response) => {

                var Respuesta = JSON.parse(response);
                if (Respuesta.Code == "OK") {
                    console.log(Respuesta);
                    this.Filtrar(1, '');
                    this.Alertas(3);
                    $('#mEliminar').modal('hide');
                } else {
                    console.log(Respuesta);
                    this.Alertas(4, Respuesta)
                    $('#mEliminar').modal('hide');
                }
            }

        });

    }

    EditarUsuario() {
        let Data = $(".Editar").serialize();
        let Rol = $('#mEditar_Rol').find(":selected").val();
        Data = Data + "&Rol=" + Rol;

        $.post(
            "Editar",
            Data,
            (response) => {
                try {
                    let Respuesta = JSON.parse(response);
                    if (Respuesta.Code == "OK") {
                        $("#mEditar").modal('hide');
                        this.Filtrar(1, '');
                        this.Alertas(2);
                    }
                } catch(ex){
                    console.log(Respuesta);
                    document.getElementById("mensaje").innerHTML = Respuesta.Description;
                }
            }
        );

    }

    GetUsuario(Id) {
        $.get(
            "Getusuario",
            { "Id": Id },
            (Response) => {
                let data = JSON.parse(Response);
                $("#mEditar_Email").val(data.Email);
                $("#mEditar_UserName").val(data.UserName);
                $("#mEditar_PhoneNumber").val(data.PhoneNumber);
                $("#mEditar_Rol").val(data.Rol);
            }
        );
    }

    BuscarUsuarios() {
        let Value = $("#filtrar").val();
        this.Filtrar(1, Value);
    }

    SaveData(Id) {
        this.Id = Id;
    }

    Alertas(value, response) {

        $(".alert").removeClass("fade").show();

        switch (value) {

            case 1:
                $(".alert").addClass("alert-success");
                $(".alert").append('<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                    '<strong>Agregado!</strong>' +
                    '<p>Se agrego el usuario a los registros correctamente.</p>');
                break;

            case 2:
                $(".alert").addClass("alert-info");
                $(".alert").append('<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                    '<strong>Editado!</strong>' +
                    '<p>Los datos nuevos se guardaron corractamente.</p>');
                break;

            case 3:
                $(".alert").addClass("alert-warning");
                $(".alert").append('<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                    '<strong>Eliminado</strong>' +
                    '<p>Usuario eliminado de los registros.</p>');
                break;

            case 4:
                $(".alert").addClass("alert-danger");
                $(".alert").append('<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                    '<strong>Error</strong>' +
                    '<p>' + response.Description + '</p>');
                break;

            default:
                break;

        }
        //continuacion
        $(".alert").delay(200).fadeOut(3000);

        setTimeout(function () {
            $(".alert").empty();
        }, 3000);


    }

    ClearModal() {
        $('#mAgregar').on('hidden.bs.modal', function (e) {
            document.getElementById("fAgregar").reset();
        });
    }
}
