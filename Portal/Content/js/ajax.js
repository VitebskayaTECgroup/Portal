function post(url, body) {
	var xhr = new XMLHttpRequest()
	xhr.open('POST', url, true)
	xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded')
	xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest")
	xhr.onreadystatechange = function () {
		if (xhr.readyState != 4) return
		if (xhr.status > 400) return alert('Ошибка HTTP: ' + xhr.status + ' [' + xhr.statusText + ']')

		try {
			var json = JSON.parse(xhr.responseText)
			if (json.Done) alert(json.Done)
			if (json.Error) alert('Ошибка сервера: ' + json.Error)
			if (json.Link) location = json.Link
			if (json.Reload) location.reload()
		}
		catch (e) {
			alert('Получен неожиданный ответ: ' + xhr.responseText)
		}
	}
	xhr.send(body)
}

function submit(url) {

	if (!url) return alert('Не передан адрес для отправки формы')

	var form = [], i = 0, items = document.querySelectorAll('input,select,textarea')
	for (; i < items.length; i++) {
		var name = items[i].name, value = items[i].type == 'checkbox' ? items[i].checked : (items[i].value || '')
		if (name) form.push(name + '=' + encodeURIComponent(value))
	}

	post(url, form.join('&'))
}

function action(url) {
	if (!url) return alert('Не передан адрес для выполнения команды')

	post(url, '')
}