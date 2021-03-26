var compromissos = {
    inicializado: false,

    init: () => {

        if (this.inicializado) {
            return;
        }

        $("#dt-tbl-compromissos").DataTable({
            'searching': false,
            "lengthChange": false,
            "dom": "<'#comp-dt-toolbar'>",
            "ajax": {
                "url": "/agendamentos/api/compromissos/profissional",
                "data": function (d) {
                    d.codProfissional = 1207;
                }
            },
            "language": {
                "info": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                "paginate": {
                    first: "Primeiro",
                    previous: "Anterior",
                    next: "Próximo",
                    last: "Último"
                }
            },
            "fnInitComplete": () => {
                $("#comp-dt-toolbar").html(compromissos.compDtToolbarHtml());
                $("#txt-data-filtro")[0].valueAsDate = new Date();

                $("#btn-p-data").click(function () {
                    $("#txt-data-filtro")[0].valueAsDate = new Date(moment($("#txt-data-filtro").val()).add(-1, 'd').format());
                    $("#txt-data-filtro").trigger('change');
                });

                $("#btn-px-data").click(function () {
                    $("#txt-data-filtro")[0].valueAsDate = new Date(moment($("#txt-data-filtro").val()).add(1, 'd').format());
                    $("#txt-data-filtro").trigger('change');
                });

                $("#sel-filtro-profissional").find("option").remove();

                $.get("/agendamentos/api/profissionais")
                    .then(function (data) {
                        $("#sel-filtro-profissional").append(`<option selected value=null></option>`);

                        data.forEach(function (item) {
                            $("#sel-filtro-profissional").append(`<option value=${item.codPessoa}>${item.nomePessoa}</option>`);
                        });
                    })
                    .fail(function () {
                        Swal.fire(
                            '',
                            'Falha ao obter lista de profissionais',
                            'error'
                        );
                    });

                $("#sel-filtro-profissional,#txt-data-filtro").change(function () {
                    $("#dt-tbl-compromissos").dataTable().api().ajax.reload(null, false);
                });
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
    },
    compDtToolbarHtml: function () {
        return `

<div class="btn-toolbar" style="margin-bottom: 10px; float: left" role="toolbar" aria-label="navegadores de data">
  <div class="btn-group btn-group-sm mr-2" role="group">
    <button type="button" id="btn-p-data" class="btn btn-outline-secondary"><</button>
    <button type="button" id="btn-px-data" class="btn btn-outline-secondary">></button>
  </div>
</div>

<div class="input-group input-group-sm mb-3" style="width: 150px; float: left;">
  <input type="date" id="txt-data-filtro" class="form-control" aria-label="Sizing example input" aria-describedby="inputGroup-sizing-sm">
</div>

<div class="btn-toolbar" style="margin-bottom: 10px; float: right" role="toolbar" aria-label="navegadores de data">
  <div class="btn-group btn-group-sm" role="group" aria-label="Third group">
    <button type="button" class="btn btn-outline-success">+</button>
  </div>
</div>


<div class="form-group" style="clear: both">
    <label for="sel-filtro-profissional">Profissional: </label>
    <select class="form-control" id="sel-filtro-profissional">
      <option>Lorena</option>
      <option>Cris Cola</option>
      <option>Heleno Sales Mesquita</option>
    </select>
</div>
`;
    },
}