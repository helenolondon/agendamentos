function compromissosCtrl() {
    var inicializado = false;
    var dtTableObj = null;

    this.init = () => {
        $.ajaxSetup({ contentType: "application/json; charset=utf-8" });

        if (inicializado) {
            return;
        }

        this.initMdCompromissos(() => {
            dtTableObj = $("#dt-tbl-compromissos").DataTable({
                'searching': false,
                "lengthChange": false,
                "dom": "<'#comp-dt-toolbar'>",
                "ajax": {
                    "url": "/agendamentos/api/compromissos/profissional",
                    "data": function (d) {
                        return $.extend({}, d, {
                            "codProfissional": $("#sel-filtro-profissional").val(),
                            "data": $("#txt-data-filtro").val(),
                        });

                    }
                },
                "language": {
                    "info": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                    "emptyTable": "Sem dados para exibir",
                    "paginate": {
                        first: "Primeiro",
                        previous: "Anterior",
                        next: "Próximo",
                        last: "Último"
                    }
                },
                "fnInitComplete": () => {

                    $("#comp-dt-toolbar").html(this.compDtToolbarHtml());
                    $("#txt-data-filtro")[0].valueAsDate = new Date();

                    $("#btn-p-data").click(function () {
                        $("#txt-data-filtro")[0].valueAsDate = new Date(moment($("#txt-data-filtro").val()).add(-1, 'd').format());
                        $("#txt-data-filtro").trigger('change');
                    });

                    $("#btn-px-data").click(function () {
                        $("#txt-data-filtro")[0].valueAsDate = new Date(moment($("#txt-data-filtro").val()).add(1, 'd').format());
                        $("#txt-data-filtro").trigger('change');
                    });

                    $("#btn-novo-compromisso").click(() => {
                        this.clearMdCompromissos();

                        $("#md-novo-compromisso").draggable();
                        $("#md-novo-compromisso").modal("show");
                    });

                    $("#sel-filtro-profissional").find("option").remove();

                    $.get("/agendamentos/api/profissionais")
                        .then(function (data) {
                            $("#sel-filtro-profissional").append(`<option selected value=0></option>`);

                            data.forEach(function (item) {
                                $("#sel-filtro-profissional").append(`<option value=${item.codPessoa}>${item.nomePessoa}</option>`);
                            });

                            $("#sel-profissional").append(`<option selected value=null></option>`);

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

                    $("#sel-filtro-profissional,#txt-data-filtro").change(() => {
                        $("#dt-tbl-compromissos").dataTable().api().ajax.reload(() => {
                            this.onReloadDataTable();
                        }, false);
                    });
                },
                "columns": [
                    { "data": "descricao", "width": '35%' },
                    {
                        "data": "inicio",
                        "width": "15%",
                        "render": function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY HH:mm");
                        }
                    },
                    {
                        "data": "termino",
                        "width": "18%",
                        "render": function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY HH:mm");
                        }
                    },
                    { "data": "tipo", "width": "18%" },
                    {
                        "data": "codCompromisso",
                        "width": "9%",

                        "orderable": false,
                        render: function (data, type, row) {
                            return "<div class='dtbuttoes'><a href='#' class='dtEditar'><i class='fas fa-edit'></i></a>&nbsp;" +
                                "<a href='#' class='dtRemover'><i class='fas fa-times-circle'></i></a></div>";
                        }
                    }
                ],
            });

            this.inicializado = true;
        });
    };
    this.onReloadDataTable = () => {
        var self = this;

        $(".dtEditar").on('click', function (e) {
            e.preventDefault();

            var row = $(this).closest('tr');

            self.onEditarCompromisso(dtTableObj.row(row).data());
        })

        $(".dtRemover").on('click', function (e) {
            e.preventDefault();
            var row = $(this).closest('tr');

            self.onRemoverCompromisso(dtTableObj.row(row).data());
        });
    };

    this.onRemoverCompromisso = (data) => {
        var md = $("#md-confirma");
        md.find(".modal-body").html("<span>Confirma a exclusão do compromisso?</span>");
        md.draggable();
        md.find("#btn-sim").off('click');
        md.find("#btn-sim").on('click', () => {
            var q = $.ajax({
                url: '/agendamentos/api/compromissos/profissional?codCompromisso=' + data.codCompromisso,
                type: 'DELETE',
                success: (result) => {
                    $("#dt-tbl-compromissos").dataTable().api().ajax.reload(() => {
                        this.onReloadDataTable();
                    }, false);
                }
            });

            $.when([q]).then(() => {
                md.modal('hide');
            })
        })

        md.modal();
    };

    this.onEditarCompromisso = (data) => {
        $("#txt-compromisso-data-inicio")[0].valueAsDate = new Date(data.inicio);
        $("#txt-compromisso-data-final")[0].valueAsDate = new Date(data.termino);

        $("#txt-compromisso-hora-inicio").val(moment(data.inicio).format("HH:mm:ss"));
        $("#txt-compromisso-hora-final").val(moment(data.termino).format("HH:mm:ss"));

        $("#txt-compromisso-descricao").val(data.descricao);
        $("#sel-profissional").val(data.codProfissional);

        $("#txt-cod-compromisso").val(data.codCompromisso);

        $("#md-novo-compromisso").draggable();
        $("#md-novo-compromisso").modal("show");
    };

    this.onCompromissoSalvo = () => { };

    this.onCompromissoSalvar = () => {
        var codCompromisso = parseInt($("#txt-cod-compromisso").val());

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

        $("#btn-salvar-compromisso")
            .attr('disabled', true)
            .html("Salvando aguarde...");

        return $.post("/agendamentos/api/compromissos/profissional", JSON.stringify(data), () => {
            $("#frm-compromisso .salvar-erros")
                .html(null);

            $("#dt-tbl-compromissos").dataTable().api().ajax.reload(() => {
                this.onReloadDataTable();
            }, false);
        })
            .fail(function (err) {
                if (err && err.status == 400) {
                    $("#frm-compromisso .salvar-erros")
                        .html(err.responseText);
                }
                else {
                    Swal.fire(
                        '',
                        'Ocorreu um erro ao salvar o compromisso',
                        'error'
                    );
                }
            })
            .always(function () {
                $("#btn-salvar-compromisso")
                    .attr('disabled', false)
                    .html("Salvar");
            })
    };

    this.clearMdCompromissos = function () {
        $("#txt-compromisso-data-inicio")[0].valueAsDate = new Date();
        $("#txt-compromisso-data-final")[0].valueAsDate = new Date();

        $("#txt-compromisso-hora-inicio").val(null);
        $("#txt-compromisso-hora-final").val(null);

        $("#txt-compromisso-descricao").val(null);
        $("#sel-profissional").val(null);
        $("#txt-cod-compromisso").val(0);

        $("#frm-compromisso .salvar-erros").html(null);
        $("#frm-compromisso")[0].classList.remove('was-validated');
    };

    this.initMdCompromissos = (readyCallBack) => {

        $("#md-compromisso-div")
            .load("/agendamentos/agendamentos/compromissoform", () => {

                var frm = $("#frm-compromisso").submit((event) => {
                    event.preventDefault();

                    if (!frm[0].checkValidity()) {

                        frm[0].classList.add('was-validated');
                        event.stopPropagation();

                        return;
                    };

                    this.onCompromissoSalvar()
                        .then(() => {
                            this.onCompromissoSalvo();
                            $('#md-novo-compromisso').modal("hide");
                        })
                });

                readyCallBack();
            })
            .ajaxError(function () {
                Swal.fire(
                    '',
                    'Ocorreu uma falha ao carregar formulário de compromissos',
                    'error'
                );
            });
    };

    this.compDtToolbarHtml = function () {
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
    };
}