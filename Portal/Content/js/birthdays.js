/* Birthdays */

var state = {
    mode: document.getElementById('mode').value,
    key: '',
    date: stringToDate(document.getElementById('day').value)
}

var picker = new DatePicker($.extend({ el: document.getElementById('day') }, pickerOptions))

document.getElementById('mode').onchange = function () {
    if (this.value === 'month') {
        document.getElementById('month').className = ''
        document.getElementById('day').className = 'hide'
    } else {
        document.getElementById('month').className = 'hide'
        document.getElementById('day').className = ''
    }
    state.mode = this.value;
    reload();
}

document.getElementById('day').onchange = picker.onSelect = function () {
    var value = this.value || this.target.value || ''
    state.date = value === '' ? new Date() : stringToDate(value)
    document.getElementById('month').value = '' + state.date.getMonth()
    reload()
}

document.getElementById('month').onchange = function () {
    state.date.setMonth(+this.value)
    document.getElementById('day').value = dateToString(state.date)
    reload()
}

document.getElementById('search').onchange = function () {
    state.key = this.value
    reload()
}

function reload() {
    $.get('/site/birthdays/query', {
        mode: state.mode,
        key: state.key,
        date: dateToString(state.date)
    }, function (data) { document.getElementById('view').innerHTML = data })
}

function stepBack() {
    if (state.mode === 'month') {
        state.date.setMonth(state.date.getMonth() - 1)
    } else {
        state.date.setDate(state.date.getDate() - 1)
    }
    document.getElementById('month').value = state.date.getMonth()
    document.getElementById('day').value = dateToString(state.date)
    reload()
}

function stepForward() {
    if (state.mode === 'month') {
        state.date.setMonth(state.date.getMonth() + 1)
    } else {
        state.date.setDate(state.date.getDate() + 1)
    }
    document.getElementById('month').value = state.date.getMonth()
    document.getElementById('day').value = dateToString(state.date)
    reload()
}

function setDefaults() {
    state.date = new Date()
    document.getElementById('month').value = state.date.getMonth()
    document.getElementById('day').value = dateToString(state.date)
    reload()
}

function clearSearch() {
    state.key = document.getElementById('search').value = ''
    reload()
}