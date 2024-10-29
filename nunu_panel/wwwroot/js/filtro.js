

function filtrar(tableID, col1, col2, col3, col4, col5) {
    // Inicializar la datatable si no está ya inicializada
    var table = $(tableID).DataTable();

    // Filtrado por texto libre
    $('#textoLibre').on('keyup', function() {
        table.search(this.value).draw();
    });

    // Índices de las columnas que se van a incluir en el offcanvas
    var columnasParaFiltrar = [];
    if (col1 !== '') columnasParaFiltrar.push(col1);
    if (col2 !== '') columnasParaFiltrar.push(col2);
    if (col3 !== '') columnasParaFiltrar.push(col3);
    if (col4 !== '') columnasParaFiltrar.push(col4);
    if (col5 !== '') columnasParaFiltrar.push(col5);

    // Llenar el offcanvas con los selectores de filtro
    table.columns().every(function(index) {
        if (columnasParaFiltrar.includes(index)) {
            var column = this;
            var columnTitle = $(column.header()).text();
            var container = $('<div class="row mt-3"></div>');
            var colDiv = $('<div class="col-md-12"></div>');
            var formGroup = $('<div class="form-group"></div>');
            var label = $('<label for="">' + columnTitle + '</label>');
            var select = $('<select name="" id="" class="form-select"><option value="">'+ columnTitle +'</option></select>');

            formGroup.append(label).append(select);
            colDiv.append(formGroup);
            container.append(colDiv);
            $('#filterForm').append(container);

            select.on('change', function() {
                var val = $.fn.dataTable.util.escapeRegex($(this).val());
                column.search(val ? '^' + val + '$' : '', true, false).draw();
            });

            var uniqueValues = getUniqueColumnValues(column);
            uniqueValues.sort().forEach(function(d) {
                select.append('<option value="' + d + '">' + d + '</option>');
            });
        }
    });


    // Aplicar filtros cuando se haga clic en el botón "Aplicar Filtros"
    $('#applyFilters').on('click', function() {
        $('#filterForm select').each(function(index) {
            var val = $.fn.dataTable.util.escapeRegex($(this).val());
            var columnIndex = columnasParaFiltrar[index];
            table.column(columnIndex).search(val ? '^' + val + '$' : '', true, false).draw();
        });
        // $('#filterModal').modal('hide');
    });

    // Limpiar filtros

    $('#clearFilters').on('click', function() {
        $('#filterForm select').val('');
        $('#textoLibre').val('');
        table.columns().search('').draw();
    });
}

// Función para obtener opciones únicas de una columna
// Función para obtener opciones únicas de una columna
function getUniqueColumnValues(column) {
    var values = [];
    column.nodes().each(function(cell) {
        var spanText = $(cell).find('span').text().trim(); // Extrae el texto del span
        if (spanText && values.indexOf(spanText) === -1) {
            values.push(spanText);
        }
    });
    return values;
}



