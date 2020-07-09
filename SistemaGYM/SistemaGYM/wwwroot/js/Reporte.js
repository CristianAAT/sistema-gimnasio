

class Reporte {

    constructor() {
        this.Id = "hola";
    }

    Reporte() {
    }

    Buscar() {
        let Mes = $("#filtrarMes").val();
        let Anio = $("#filtrarAnio").val();
        this.Reportes(Mes, Anio);
    }

    Reportes(Mes, Anio) {
        $.post(
            "GetReportes",
            { Mes, Anio },
            (response) => {
                $("#ResultTable").html(response);
            }

        );
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

    Fecha() {
        const monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Deciembre"
        ];

        var date = new Date();
        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();


        //Slects de busqueda

        monthNames.forEach(function (item, val) {
            let Select = $('#filtrarMes');
            let Option = document.createElement("option");
            Option.value = val+1;
            Option.textContent = item;
            Select.append(Option);
        });
        $("#filtrarMes").val(month);

        for (var i = 0; i < 12; i++) {
            let Select = $('#filtrarAnio');
            let Option = document.createElement("option");
            Option.value = year-i;
            Option.textContent = year-i;
            Select.append(Option);
        }
        $("#filtrarAnio").val(year);
    }

    GetMonths() {
        $.get(
            "GetDataMonths",
            (response) => {
                google.charts.load("visualization", "1", { packages: ["corechart"] });
                google.charts.setOnLoadCallback(DrawVisualization);
                function DrawVisualization() {
                    let Table = new google.visualization.DataTable();
                    Table.addColumn('string', 'Mes');
                    Table.addColumn('number', 'Ingreso por mensualidades');
                    Table.addColumn('number', 'Ingreso por inscripciones');
                    Table.addColumn('number', 'Ingreso por visitas');


                    var Json = JSON.parse(response);

                    $.each(Json, function (index, item) {
                        Table.addRows([[item.Mes, item.Mensualidad, item.Inscripcion, item.Visitas]]);
                    });

                    var options = {
                        'title': 'Ventas en el ultimo año',
                        isStacked: true,
                        bar: { groupWidth: '75%' },
                        height: 350
                    };


                    let Chart = new google.visualization.ColumnChart(document.getElementById("Ingresos"));
                    Chart.draw(Table, options);
                }
            }
        );


    }
}
