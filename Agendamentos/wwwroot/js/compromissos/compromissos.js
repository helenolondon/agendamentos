var compromissos = {
    inicializado: false,

    init: () => {

        if (this.inicializado) {
            return;
        }

        $("#dt-tbl-compromissos").DataTable({
            'searching': false,
            "lengthChange": false,
            "ajax": {
                "url": "/agendamentos/api/compromissos/profissional",
                "data": function (d) {
                    d.codProfissional = 1207;
                }
            },
            "language": {
                "info": "Mostrando de _START_ até _END_ de _TOTAL_ registros"
            },
            "fnInitComplete": function () {
            },
            "columns": [
                { "data": "descricao" },
                {
                    "data": "inicio",
                    "render": function (data, type, row) {
                        return moment(data).format("DD/MM/YYYY HH:mm");
                    }
                },
                {
                    "data": "termino",
                    "render": function (data, type, row) {
                        return moment(data).format("DD/MM/YYYY HH:mm");
                    }
                },
                { "data": "tipo"},
            ],
        });

        this.inicializado = true;
    }
}