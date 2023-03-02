var Auto = {

    reload: function (id) {
        var model = {
            date: document.getElementById('auto-date').value,
            self: document.getElementById('self').checked,
            mode: document.getElementById('mode').value,
            id: id
        }

        $.post(source + '/list', model, function (data) {
            document.getElementById('view').innerHTML = data
        })
    },

    clear: function () {
        document.getElementById('self').checked = false
        document.getElementById('mode').value = 'all'
        document.getElementById('auto-date').value = dateToString(new Date())

        Auto.reload()
    },

    create: function () {
        $.get(source + '/form', function (data) {
            $('#form').html(data).slideDown(100)
            Auto.initForm()
        })
    },

    update: function (id) {
        $.get(source + '/form/' + id, function (data) {
            $('#auto-' + id).slideUp(100)
            $('#form').html(data).slideDown(100)
            Auto.initForm()
        })
    },

    collect: function () {
        return {
            data: document.getElementById('data').value,
            start: document.getElementById('start_time').value,
            end: document.getElementById('end_time').value,
            adress: document.getElementById('adress').value,
            target: document.getElementById('target').value
        }
    },

    callback: function (data) {
        if (data.indexOf('auto_errors') > -1) {
            document.getElementById('errors').innerHTML = data
        } else {
            if (document.getElementById('auto-date').value !== data) {
                document.getElementById('auto-date').value = data
            }
            Auto.reload()
        }
    },

    endCreate: function () {
        $.post(source + '/create', Auto.collect(), Auto.callback)
    },

    endUpdate: function (id) {
        $.post(source + '/update/' + id, Auto.collect(), Auto.callback)
    },

    del: function (id) {
        if (confirm('Заявка будет удалена без возможности восстановления. Продолжить?')) {
            $.post(source + '/delete/' + id, function () {
                $('#auto-' + id).slideUp(100, function () {
                    $('#auto-' + id).remove()
                })
            })
        }
    },

    close: function (id) {
        $('#form').slideUp(100, function () {
            document.getElementById('form').innerHTML = ''
        })
        $('#auto-' + id).slideDown(100)
    },

    answer: function (id, obj) {
        $.get(source + '/answerform/' + id, function (data) {
            $(obj).closest('.auto_block').find('.auto_answer').html(data).show()
            obj.style.display = 'none'
            Auto.initForm()
        })
    },

    answerChange: function (obj) {
        var $block = $(obj).closest('.auto_block');
        $block.find('.answer').hide()
        $block.find('.answer_' + obj.value).show()
    },

    closeAnswer: function (obj) {
        $block = $(obj).closest('.auto_block')
        $block.find('button').show()
        $block.find('.auto_answer').hide().html('')
    },

    endAnswer: function (id, obj) {
        var $block = $(obj).closest('.auto_block')
        var value = $block.find('#answerSelect').val()
        var model = {
            answerCode: value
        }

        if (value == 1) {
            model['acceptDate'] = $block.find('#acceptDate').val()
            model['carId'] = $block.find('#carId').val()
            model['driverId'] = $block.find('#driverId').val()
            model['pathNumber'] = $block.find('#pathNumber').val()
            model['comments'] = $block.find('#comments1').val()
        }

        if (value == -1) {
            model['comments'] = $block.find('#comments2').val()
        }

        $.post(source + '/answer/' + id, model, function (data) {
            if (data.indexOf('error') > -1) {
                $block.find('#answerErrors').html(data)
            } else {
                Auto.reload()
            }
        })
    },

    copy: function (id) {
        $.get(source + '/copy/' + id, function (data) {
            $('#form').html(data).slideDown(100)
            Auto.initForm()
        })
    },

    print: function () {
        $.get(source + '/print', function (data) {
            var a = document.createElement('a')
            document.body.appendChild(a)
            a.className = 'hide'
            a.href = '/site/content/documents/' + data
            a.click()
            document.body.removeChild(a)
        })
    },

    step: function (direction) {

        var str = document.getElementById('auto-date').value
        var date = stringToDate(str)

        date.setDate(date.getDate() + direction)
        document.getElementById('auto-date').value = dateToString(date)

        Auto.reload()
    },

    initForm: function () {

        var datepicks = document.querySelectorAll('.datepick')
        for (var i = 0; i < datepicks.length; i++) {
            new DatePicker({
                el: datepicks[i]
            })
        }

        $(".timepick").timepicker({
            "minTime": "8:00am",
            "maxTime": "5:00pm",
            "timeFormat": 'H:i',
            "step": 15
        });
    }
};

var Cars = {
    collect: function (obj) {
        var inputs = obj.parentNode.parentNode.getElementsByTagName('input')
        return {
            Model: inputs[0].value,
            Number: inputs[1].value,
            Type: inputs[2].value
        }
    },
    create: function (obj) {
        $.post(source.replace('/cars', '/createcar'), Cars.collect(obj), function () { location.reload() })
    },
    save: function (id, obj) {
        $.post(source.replace('/cars', '/savecar/' + id), Cars.collect(obj), function () { location.reload() })
    },
    del: function (id) {
        $.post(source.replace('/cars', '/deletecar/' + id), function () { location.reload() })
    }
};

var Drivers = {
    collect: function (obj) {
        var inputs = obj.parentNode.parentNode.querySelectorAll('input,select')
        return {
            Name: inputs[0].value,
            PhoneNumber: inputs[1].value,
            DefaultCarId: inputs[2].value
        }
    },
    create: function (obj) {
        $.post(source.replace('/drivers', '/createdriver'), Drivers.collect(obj), function () { location.reload() })
    },
    save: function (id, obj) {
        $.post(source.replace('/drivers', '/savedriver/' + id), Drivers.collect(obj), function () { location.reload() })
    },
    del: function (id) {
        $.post(source.replace('/drivers', '/deletedriver/' + id), function () { location.reload() })
    }
}