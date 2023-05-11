'use strict';


$(window).on('error', function (e) {
	$.post(host + 'site/error', {
		page: document.location.pathname.replace(/\//g, ''),
		error: e.originalEvent.error.stack || e.originalEvent.message || e
	})
})


function printReportCart() {
	$.get('/devin/devices/PrintRecordCartByIp?Ip=' + document.getElementById('hostName').innerHTML, function (json) {
		if (json.Error) alert(json.Error)
		if (json.Link) {
			var a = document.createElement('a')
			document.body.appendChild(a)
			a.href = json.Link
			a.click()
		}
	})
}

function toggleMenu(obj, name) {
	var $menu = $(obj).closest('.menu')
	var $body = $menu.find('.menu_body')
	var $arrow = $menu.find('.menu_arrow div')
	var state = ''
	if ($menu.hasClass('open')) {
		$body.slideUp(100, function () {
			$menu.removeClass('open')
		})
		state = 0
	} else {
		$body.slideDown(100)
		$menu.addClass('open')
		state = 1
	}
	setCookie(name, state)
	$arrow.toggleClass('ic-arrow-up ic-arrow-down')
}

function getCookie(name) {
	var matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"))
	return matches ? decodeURIComponent(matches[1]) : undefined
}

function setCookie(name, value, options) {
	options = options || { expires: 99999999999 }
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

$(document)
	.on('mouseenter', '.widget_active', function () {
		$(this).animate({ height: '+=20px'}, 100).find('.widget_bottom_link').css('display', 'block')
	})
	.on('mouseleave', '.widget_active', function () {
		$(this).animate({ height: '-=20px' }, 100).find('.widget_bottom_link').css('display', 'none')
	})

var pickerOptions = {
	format: 'dd.MM.yyyy',
	i18n: {
		months: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
		weeks: ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс']
	}
}

$(document).on('click', 'th[data-type]', function () { _sort(this) })

function _sort(th) {

	function to_date(s) {
		if (s.indexOf(' ') > -1) {
			var t = s.split(' ')
			var tdate = t[0].split('.')
			var ttime = t[1].split(':')
			return new Date(+tdate[2], +tdate[1], +tdate[0], +ttime[0], +ttime[1], +ttime[2])
		}
		else if (s.indexOf('.') > -1) {
			var x = s.split('.')
			return new Date(+x[2], +x[1], +x[0])
		}
		else {
			var u = s.split(':')
			return new Date(2000, 1, 1, +u[0], +u[1], +u[2])
		}
	}

	var $table = $(th).closest('table'),
		$body = $table.find('tbody'),
		rowsArray = [],
		type = th.getAttribute('data-type'),
		way = th.getAttribute('data-way'),
		colNum = th.cellIndex,
		compare = function () {}

	$.each($body.find('tr'), function () { rowsArray.push(this) })

	switch (type) {

		case 'number':
			compare = function (a, b) {
				return way === 'up'
					? Number(a.cells[colNum].innerHTML.replace(',', '.')) - Number(b.cells[colNum].innerHTML.replace(',', '.'))
					: Number(b.cells[colNum].innerHTML.replace(',', '.')) - Number(a.cells[colNum].innerHTML.replace(',', '.'))
			}
			break

		case 'date':
			compare = function (a, b) {
				var A = +to_date(a.cells[colNum].innerHTML)
				var B = +to_date(b.cells[colNum].innerHTML)
				return way === 'up'
					? B > A ? 1 : -1
					: A > B ? 1 : -1
			}
			break

		case 'type':
			compare = function (a, b) {
				return way === 'up'
					? ($(a.cells[colNum]).find('div').attr('class') || '') > ($(b.cells[colNum]).find('div').attr('class') || '') ? 1 : -1
					: ($(b.cells[colNum]).find('div').attr('class') || '') > ($(a.cells[colNum]).find('div').attr('class') || '') ? 1 : -1
			}
			break

		default:
			compare = function (a, b) {
				return way === 'up'
					? a.cells[colNum].innerHTML > b.cells[colNum].innerHTML ? 1 : -1
					: b.cells[colNum].innerHTML > a.cells[colNum].innerHTML ? 1 : -1
			}
			break
	}

	$table.find('thead th').each(function () { this.className = '' })

	if (way === 'up') {
		th.setAttribute('data-way', 'down')
		th.className = 'sort_down'
	} else {
		th.setAttribute('data-way', 'up')
		th.className = 'sort_up'
	}

	rowsArray.sort(compare)

	$body.remove()
	var $tbody = $('<tbody></tbody>')
	for (var i = 0; i < rowsArray.length; i++) {
		$tbody.append(rowsArray[i])
	}
	$table.append($tbody)
}


/* News */

function viewNews() {

	if (document.getElementById('news-filter')) {

		var filter = {},
			items = document.getElementById('news-filter').querySelectorAll('input,select,textarea')

		for (var i = 0; i < items.length; i++) filter[items[i].name] = items[i].type === 'checkbox' ? items[i].checked : items[i].value

		document.title = 'Новости | Витебская ТЭЦ'
		elements('view')
		$.post(host + 'news/list?source=', filter, function (data) { $('#view').html(data) })

	} else {
		$.get(host + 'news/list', { source: 'main' }, function (data) { document.getElementById('view').innerHTML = data })
	}
}

function expandNews(obj) {
	var $news = $(obj).closest('.news'),
		$body = $news.children('.news-body'),
		$icon = $news.children('table').find('.arrow')

	if ($news.hasClass('news-first')) {
		$.get(host + 'news/body/' + $news.attr('id'), function (data) {
			$body.html(data).slideDown(150)
			$news.addClass('news-open').removeClass('news-first')
			$icon.addClass('ic-arrow-up').removeClass('ic-arrow-down')
		})
	} else {
		$body.slideToggle(150)
		$news.toggleClass('news-open')
		$icon.toggleClass('ic-arrow-down ic-arrow-up')
	}
}



function watchNews(obj, id) {
	var $news = $(obj).closest('.news')
	$.post(host + 'news/watch', { id: id }, function () {
		if ($news.hasClass('news-back')) {
			location.reload()
		}
		else {
			obj.onclick = function () { return false; }
			$news.removeClass('news-unwatched news-blink')
		}
	})
}

setInterval(function () {
	$('.news-unwatched').toggleClass('news-blink')
}, 1000)