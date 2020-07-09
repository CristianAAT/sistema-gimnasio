

class Membresia {

    constructor() {
        this.Id = "hola";
    }

    Membresia() {
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

    Agregar() {
        document.getElementById("mensaje").innerHTML = "";
        let Data = $(".Agregar").serialize();
        let Socio = $('#Socios').find(":selected").val();
        Data = Data + "&SocioID=" + Socio;

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
        console.log(this.Id);
        $.ajax({
            url: "Eliminar",
            type: "DELETE",
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

    Buscar() {
        let Value = $("#filtrar").val();
        this.Filtrar(1, Value);
    }

    BuscarSocio() {
        let Value = $("#InputBuscarSocios").val();
        
        $('#Socios')
            .empty()
            .append('<option selected="selected" value="null">Selecciona uno</option>');

        $.post(
            "Buscarsocios",
            { "Value": Value },
            (response) => {
                console.log(response);

                response.forEach(function (item, val) {
                    let Select = $('#Socios');
                    let Option = document.createElement("option");
                    Option.value = item.value;
                    Option.textContent = item.text;
                    Select.append(Option);
                }, this);
            }

        );
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
                    '<p>Se agrego una nueva membresia!.</p>');
                break;

            case 2:
                $(".alert").addClass("alert-info");
                $(".alert").append('<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                    '<strong>Editado!</strong>' +
                    '<p>Los datos de la membresia de editaron correctamente.</p>');
                break;

            case 3:
                $(".alert").addClass("alert-warning");
                $(".alert").append('<button type="button" class="close" data-dismiss="alert">&times;</button>' +
                    '<strong>Eliminado</strong>' +
                    '<p>Resgistro eliminado.</p>');
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
        document.getElementById('StartDate').setAttribute('value', value);
        //$("#StartDate").val(value);
    }
}
