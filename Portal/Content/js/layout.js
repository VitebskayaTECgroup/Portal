'use strict';

window.addEventListener('error', function (ev) {
	var form = new FormData()
	form.append('page', document.location.pathname.replace(/\//g, ''))
	form.append('error', ev.originalEvent.error.stack || ev.originalEvent.message || ev)

	fetch(host + 'site/error', { method: 'POST', body: form })
})

document.querySelectorAll('.nav-button').forEach(function (el) {
	if (el.hasAttribute('unactive')) return
	var timeout = 0
	el.addEventListener('mouseenter', function () {
		timeout = setTimeout(function () {
			el.classList.add('visible')
			document.querySelector('.body').classList.add('visible')
		}, 250)
	})
	el.addEventListener('mouseleave', function () {
		clearTimeout(timeout)
		el.classList.remove('visible')
		document.querySelector('.body').classList.remove('visible')
	})
})

/**
 * 
 * @param {HTMLDivElement} obj
 * @param {string} name
 */
function toggleMenu(obj, name) {

	var menu = obj.closest('.menu')
	var arrow = menu.querySelector('.menu_arrow .material-icons')

	var state = ''
	if (menu.classList.contains('open')) {
		menu.classList.remove('open')
		arrow.innerHTML = 'expand_more'
		state = 0
	} else {
		menu.classList.add('open')
		arrow.innerHTML = 'expand_less'
		state = 1
	}
	setCookie(name, state, { expires: 9999999999 })
}

function setTheme() {
	document.body.classList.toggle('white')
	document.body.classList.toggle('dark')
	setCookie('theme', document.body.classList.contains('dark') ? 'dark' : 'white', { expires: 999999999999, path: '/' })
}

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

function dateToString(date, format) {
	format = format || pickerOptions.format
	date = date || new Date()
	if (!isValidDate(date)) return 'Invalid Date'
	return format
		.replace('yyyy', '' + date.getFullYear())
		.replace('MM', to2(date.getMonth() + 1))
		.replace('dd', to2(date.getDate()))
		.replace('hh', to2(date.getHours()))
		.replace('mm', to2(date.getMinutes()))
	function to2(n) { return (n < 10 ? '0' : '') + n }
}

function stringToDate(str, format) {
	str = str || ''
	format = format || pickerOptions.format || 'dd.MM.yyyy'

	var date = new Date(sub('yyyy'), sub('MM') - 1, sub('dd'), sub('hh'), sub('mm'))
	if (isValidDate(date)) return date

	function sub(token) {
		var ch = 0;
		if ((ch = format.indexOf(token)) > -1) {
			var n = Number(str.slice(ch, ch + token.length))
			return isNaN(n) ? 0 : n
		} else return 0
	}
}

function isValidDate(date) { return date instanceof Date && !isNaN(date.getTime()) }

var pickerOptions = {
	format: 'dd.MM.yyyy',
	i18n: {
		months: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
		weeks: ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс']
	}
}