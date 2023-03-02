//  Отображение форм
$('[toggle-el]').on('click', function () {
	let selector = this.getAttribute('toggle-el')

	let first = document.querySelector('[toggle="' + selector + '"]')
	setCookie(selector, first.style.display == 'none' ? '1' : '0')

	$('[toggle="' + selector + '"]').fadeToggle(150)
	$('[toggle-fast="' + selector + '"]').toggle()
})

// Отправка форм на сервер
$('[submit]').on('click', function () {
	let container = this.closest('[post]')
	let url = container.getAttribute('post')
	let form = new FormData()

	container.querySelectorAll('input,select,textarea').forEach(x => {
		switch (x.type) {
			case 'checkbox':
				form.append(x.name, x.checked)
				break
			case 'file':
				if (x.files.length == 0) return alert('Файл не выбран!')
				form.append(x.name, x.files[0])
				break
			default:
				form.append(x.name, x.value)
				break
		}
	})

	fetch(url, { method: 'POST', body: form })
		.then(res => res.text())
		.then(text => {
			if (text == 'done') location.reload()
			else alert(text)
		})
})

// Отображение выбранного файла в поле выбора файла
$('input[type="file"]').on('change', function () {
	$('label[for="' + this.id + '"]')
		.text(this.files.length == 0 ? 'Выберите файл' : this.files[0].name)
})

// Работа с куками
function getCookie(name) {
	var matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"))
	return matches ? decodeURIComponent(matches[1]) : undefined
}

function setCookie(name, value, options) {
	options = options || {}
	var expires = options.expires
	options.SameSite = "Lax"
	if (typeof expires == "number" && expires) {
		var d = new Date()
		d.setTime(d.getTime() + expires * 1000)
		expires = options.expires = d
	}
	if (expires && expires.toUTCString) options.expires = expires.toUTCString()
	value = encodeURIComponent(value)
	var updatedCookie = name + "=" + value
	for (var propName in options) {
		updatedCookie += "; " + propName
		var propValue = options[propName]
		if (propValue !== true) updatedCookie += "=" + propValue
	}
	document.cookie = updatedCookie
}