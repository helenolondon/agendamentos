var compromissos = {
    inicializado: false,

    init: () => {
        $.ajaxSetup({ contentType: "application/json; charset=utf-8" });

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

                $("#btn-novo-compromisso").click(() => {
                    compromissos.clearMdCompromissos();

                    $("#md-novo-compromisso").draggable();
                    $("#md-novo-compromisso").modal("show");
                });

                compromissos.initMdCompromissos();

                $.get("/agendamentos/api/profissionais")
                    .then(function (data) {
                        $("#sel-filtro-profissional").append(`<option selected value=null></option>`);

                        data.forEach(function (item) {
                            $("#sel-filtro-profissional").append(`<option value=${item.codPessoa}>${item.nomePessoa}</option>`);
                        });

                        $("#sel-filtro-profissional").append(`<option selected value=null></option>`);

                        data.forEach(function (item) {
                            $("#sel-profissional").append(`<option value=${item.codPessoa}>${item.nomePessoa}</option>`);
                        });

                        $("#sel-profissional").append(`<option selected value=null></option>`);
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
                { "data": "tipo" },
            ],
        });

        this.inicializado = true;
    },
    onCompromissoSalvo: () => { },
    onCompromissoSalvar: function () {
        var codCompromisso = $("#txt-cod-compromisso").val();

        if (codCompromisso == null || codCompromisso == undefined) {
            codCompromisso = 0;
        }

        var data = {
            "codCompromisso": codCompromisso,
            "inicio": $("#txt-compromisso-data-inicio").val() + "T" + $("#txt-compromisso-hora-inicio").val(),
            "termino": $("#txt-compromisso-data-final").val() + "T" + $("#txt-compromisso-hora-final").val(),
            "codTipo": parseInt($("#sel-tipo-compromisso").val()),
            "descricao": $("#txt-compromisso-descricao").val(),
            "codProfissional": parseInt($("#sel-profissional").val())
        }
        console.log(data);

        return $.post("/agendamentos/api/compromissos/profissional", JSON.stringify(data))
            .fail(function () {
                Swal.fire(
                    '',
                    'Ocorreu um falha ao tentar salvar o compromisso',
                    'error'
                );
            });
    },

    clearMdCompromissos: function () {
        $("#txt-compromisso-data-inicio")[0].valueAsDate = new Date();
        $("#txt-compromisso-data-final")[0].valueAsDate = new Date();

        $("#txt-compromisso-hora-inicio").val(null);
        $("#txt-compromisso-hora-final").val(null);

        $("#txt-compromisso-descricao").val(null);
        $("#sel-profissional").val(null);
    },
    initMdCompromissos: function () {

        var frm = $("#frm-compromisso").submit(function (event) {
            event.preventDefault();

            if (!frm[0].checkValidity()) {

                frm[0].classList.add('was-validated');
                event.stopPropagation();

                return;
            };

            compromissos.onCompromissoSalvar()
                .then(() => {
                    compromissos.onCompromissoSalvo();
                    $('#md-novo-compromisso').modal("hide");
                })

            event.preventDefault();
        });
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
    <button type="button" id="btn-novo-compromisso" class="btn btn-outline-success">+</button>
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