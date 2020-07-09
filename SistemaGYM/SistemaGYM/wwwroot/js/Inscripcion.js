

class Inscripcion {
    Inscripcion() { }

    constructor() {
        this.Id = "";
        this.image = null;

    }

    Filtrar(Pagina, Value) {

        Value = (Value == "") ? "null" : Value;
        let Registros = $("#RegsPerPag").val();

        $.post(
            "Filtrar",
            { Pagina, Registros, Value },
            (response) => {
                $("#ResultDeck").html(response[2]);
                $("#NavegadorDeck").html(response[1]);
                $("#InfoDeck").html(response[0]);
            }

        );
    }

    Agregar() {
        document.getElementById("mensaje").innerHTML = "";
        var Data = $(".Agregar").serialize();
        var inputFile = $("#inputFile")[0].files[0];
        var file = new FormData();
        file.append('file', inputFile);

        var Avatar;
        //upload image
        $.ajax({
            url: "Imagen",
            data: file,
            method: "POST",
            processData: false,
            contentType: false,
            async: false,
            success: (response) => {
                try {
                    var Respuesta = JSON.parse(response);
                    if (Respuesta.Code == "OK") {
                        Avatar = Respuesta.Description;
                        console.log(Respuesta);
                    } else {
                        console.log(Respuesta);
                        document.getElementById("mensaje").innerHTML = Respuesta.Description;
                    }
                } catch (ex) {
                    console.log(ex);
                }
            }
        });

        Data = Data + "&PictureAvatar=" + Avatar;
        console.log(Data)
        
        //upload data
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

    Eliminar() {
        $.ajax({
            url: "Eliminar",
            type: "POST",
            data: { "Id": this.Id },
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

    Editar() {
        document.getElementById("mensaje").innerHTML = "";
        var Data = $(".Editar").serialize();
        var inputFile = $("#inputFileEditar")[0].files[0];
        var file = new FormData();
        file.append('file', inputFile);

        var Avatar;
        //upload image
        $.ajax({
            url: "Imagen",
            data: file,
            method: "POST",
            processData: false,
            contentType: false,
            async: false,
            success: (response) => {
                try {
                    var Respuesta = JSON.parse(response);
                    if (Respuesta.Code == "OK") {
                        Avatar = Respuesta.Description;
                    } else {
                        document.getElementById("mensaje").innerHTML = Respuesta.Description;
                    }
                } catch (ex) {
                    console.log(ex);
                }
            }
        });

        Data = Data + "&PictureAvatar=" + Avatar;
        console.log(Data)

        //upload data
        $.post(
            "Editar",
            Data,
            (response) => {
                try {
                    var Respuesta = JSON.parse(response);
                    if (Respuesta.Code == "OK") {
                        console.log(Respuesta);
                        $('#mEditar').modal('hide');
                        this.Filtrar(1, '');
                        this.Alertas(2);
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

    GetSocio(Id) {
        $.get(
            "GetSocio",
            { "Id": Id },
            (Response) => {
                let data = JSON.parse(Response);
                $("#mEditar_SocioID").val(data.SocioID);
                $("#mEditar_FullName").val(data.FullName);
                $("#mEditar_Email").val(data.Email);
                $("#mEditar_PhoneNumber").val(data.PhoneNumber);
                $("#mEditar_InscriptionCost").val(data.InscriptionCost);

                //fechas
                document.getElementById('mEditar_BirthDate').setAttribute('value', data.BirthDate);
                document.getElementById('mEditar_InscriptionDate').setAttribute('value', data.InscriptionDate);
                document.getElementById('mEditar_InscriptionExpiration').setAttribute('value', data.InscriptionExpiration);
               

                //foto
                this.image = new Image();
                this.image = document.getElementById('mEditar_Output');
                this.image.src = data.PictureAvatar;
            }
        );
    }

    Buscar() {
        let Value = $("#filtrar").val();
        this.Filtrar(1, Value);
    }

    Fecha() {
        var date = new Date();
        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();

        if (day < 10)
            day = "0" + day;
        if (month < 10)
            month = "0" + month;

        var value = year + "-" + month + "-" + day;


        document.getElementById('dateBirth').setAttribute('value', "2005-01-01");
        document.getElementById('dateBirth').setAttribute('min', "1950-01-01");
        document.getElementById('dateBirth').setAttribute('max', "2005-01-01");

        document.getElementById('dateInscripcion').setAttribute('value', value);

        document.getElementById('dateExpiration').setAttribute('value', value);
        document.getElementById('dateExpiration').setAttribute('min', value);
    }

    Alertas(value, response) {

        $(".alert").removeClass("fade").show();

        switch (value) {

            case 1:
                $(".alert").addClass("alert-success");
                $(".alert").append('<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                    '<strong>Agregado!</strong>' +
                    '<p>Nuevo socio añadido.</p>');
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
                    '<p>Socio eliminado de los registros.</p>');
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

    LoadFile(event) {
        this.image = new Image();
        this.image = document.getElementById('output');
        this.image.src = URL.createObjectURL(event.target.files[0]);
    }

    LoadFileEditar(event) {
        this.image = new Image();
        this.image = document.getElementById('mEditar_Output');
        this.image.src = URL.createObjectURL(event.target.files[0]);
    }

    SaveData(Id) {
        this.Id = Id;
    }
}
