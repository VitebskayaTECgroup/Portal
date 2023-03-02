var DatePicker = (function () {

	function DatePicker(options) {

		options = options || {}
		this.target = options.el.length ? document.querySelector(options.el) : options.el
		if (!this.target) return console.error('Не передан родительский элемент')

		this.height   = 0
		this.width    = 0
		this.el       = document.createElement('div')
		this.static   = this.target.tagName != 'INPUT'
		this.format   = options.format   || 'dd.MM.yyyy'
		this.onSelect = options.onSelect || function () { }
		this.i18n     = options.i18n     || {
			months: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
			weeks: ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс']
		}
		this.date     = options.date     || stringToDate(this.target.value, this.format) || new Date()
		this.viewDate = new Date(this.date.getFullYear(), this.date.getMonth())
		this.el.className = 'picker'

		var self = this

		if (this.static) {
			this.target.appendChild(this.el)
			this.el.style.display = 'inline-block'
			this.render()
		} else {
			document.body.appendChild(this.el)
		}

		on(this.el, 'click', function (e) {

			e = e || window.event
			var src = e.target || e.srcElement

			if (src.hasAttribute('prev')) {
				self.viewDate.setMonth(self.viewDate.getMonth() - 1)
				self.date = new Date(self.viewDate.getFullYear(), self.viewDate.getMonth(), +src.innerHTML)
				if (!self.static) {
					self.target.value = dateToString(self.date, self.format)
					self.hide()
				} else {
					self.render()
				}
			}

			if (src.hasAttribute('next')) {
				self.viewDate.setMonth(self.viewDate.getMonth() + 1)
				self.date = new Date(self.viewDate.getFullYear(), self.viewDate.getMonth(), +src.innerHTML)
				if (!self.static) {
					self.target.value = dateToString(self.date, self.format)
					self.hide()
				} else {
					self.render()
				}
			}

			if (src.hasAttribute('day')) {
				self.date = new Date(self.viewDate.getFullYear(), self.viewDate.getMonth(), +src.innerHTML)
				if (!self.static) {
					self.target.value = dateToString(self.date, self.format)
					self.hide()
				} else {
					self.render()
				}
				self.onSelect(self.target, self.date)
			}

			if (src.hasAttribute('prevmonth')) {
				console.log('prevmonth')
				self.viewDate = new Date(self.viewDate.getFullYear(), self.viewDate.getMonth() - 1, 1)
				self.render()
			}

			if (src.hasAttribute('nextmonth')) {
				self.viewDate = new Date(self.viewDate.getFullYear(), self.viewDate.getMonth() + 1, 1)
				self.render()
			}
		})

		on(this.el, 'change', function (e) {

			e = e || window.event
			var src = e.target || e.srcElement

			if (src.hasAttribute('months')) {
				self.viewDate.setMonth(+e.target.value)
				self.render()
			}

			if (src.hasAttribute('years')) {
				self.viewDate.setFullYear(+e.target.value)
				self.render()
			}
		})

		if (!this.static) {

			// Реагирование на события поля ввода
			on(this.target, 'focus', function () {
				self.show()
			})

			on(this.target, 'change', function () {
				var date = stringToDate(self.target.value, self.format)
				self.date = date || self.date
				self.target.value = dateToString(self.date)
				self.render()
			})

			// Скрытие пикера при выходе за пределы поля ввода

			on(document, 'click', function (e) {
				e = e || window.event
				var src = e.target || e.srcElement
				var hide = true
				while (true) {
					if (src == self.el || src == self.target) {
						hide = false
						break
					}
					if (!src) hide = false
					if (!src || src.tagName == 'HTML') break
					else src = src.parentNode
				}
				if (hide) self.hide()
			})

			// Изменение позиционирования при изменении размера окна

			on(window, 'resize', function () {
				if (self.el.style.display !== 'none') self.position()
			})
		}
	}


	DatePicker.prototype.show = function () {
		this.date = stringToDate(this.target.value, this.format) || new Date()
		this.render()
		if (this.static) this.el.style.display = 'inline-block' 
		else {
			this.position()
			this.el.style.display = 'block'
		}
	}

	DatePicker.prototype.hide = function () {
		this.el.style.display = 'none'
	}

	DatePicker.prototype.render = function () {

		var now       = new Date(),
			date      = this.viewDate,
			year      = date.getFullYear(),
			month     = date.getMonth(),
			isPickedMonth = this.date.getFullYear() == this.viewDate.getFullYear() && this.date.getMonth() == this.viewDate.getMonth(),
			isToday   = now.getFullYear() === year && now.getMonth() === month,
			weekDay   = new Date(year, month).getDay(),
			daysCount = new Date(year, month + 1, 0).getDate(),
			current   = new Date(year, month, 0).getDate(),
			html      = '',
			years     = '',
			months    = '',
			i

		if (weekDay == 0) weekDay = 7
		for (i = 1; i < weekDay; i++) {
			html = '<td prev class="picker_another">' + (current--) + '</td>' + html
		}

		current = weekDay - 1
		for (i = 1; i < daysCount + 1; i++) {
			if (current === 0 && i !== 1) html += '</tr><tr>'

			html += '<td day class="' + (isPickedMonth && i === this.date.getDate() ? ' picker_picked' : '') + (isToday && i === now.getDate() ? ' picker_today' : '') + '">' + i + '</td>'
			current === 6 ? current = 0 : current++
		}

		if (current !== 0) {
			for (i = 1; current < 7; current++) {
				html += '<td next class="picker_another">' + (i++) + '</td>'
			}
		}

		for (i = 0; i < 12; i++) {
			months += '<option value="' + i + '"' + (i === month ? ' selected' : '') + '>' + this.i18n.months[i] + '</option>'
		}
		for (i = -5; i < 10; i++) {
			years  += '<option value="' + (year - i) + '"' + (i === 0 ? 'selected' : '') + '>' + (year - i) + '</option>'
		}

		this.el.innerHTML =
		'<table>' +
			'<thead>' +
				'<tr>' +
					'<td class="picker_arrow" prevmonth>&laquo;</td>' +
					'<td class="picker_title" colspan="5">' +
						'<select months class="picker_select">' + months + '</select>' +
						'<select years class="picker_select">' + years + '</select>' +
					'</td>' +
					'<td class="picker_arrow" nextmonth>&raquo;</td>' +
				'</tr>' +
				'<tr><th>' + this.i18n.weeks.join('</th><th>') + '</th></tr>' +
			'</thead>' +
			'<tbody>' +
				'<tr>' + html + '</tr>' +
			'</tbody>'
		'</table>'
	}

	DatePicker.prototype.position = function () {

		if (this.height === 0 && this.width === 0) {
			this.el.style.cssText =
				'display: block;' +
				'position: absolute;' +
				'visibility: hidden;'
			var rect    = this.el.getBoundingClientRect()
			this.height = rect.bottom - rect.top
			this.width  = rect.right - rect.left
		}

		var rect = this.target.getBoundingClientRect()
		var h = window.innerHeight
			|| document.documentElement.clientHeight
			|| document.body.clientHeight
		var w = window.innerWidth
			|| document.documentElement.clientWidth
			|| document.body.clientWidth
		var top  = rect.bottom + 2
		var left = rect.left

		if (left + this.width > w) left = (w - this.width - 5)
		if (top + this.height > h) top = (h - this.height - 5)
		if (rect.left <= 0) left = 5
		if (rect.top <= 0) top = 5

		this.el.style.cssText = 'position:absolute;top:' + top + 'px;left:' + left + 'px;'
	}

	var
		listener = !!window.addEventListener,

		isValidDate = function (date) {
			return date instanceof Date && !isNaN(date.getTime())
		},

		stringToDate = function (str, format) {
			if (!str || str === '') return

			var date = new Date(sub('yyyy'), sub('MM') - 1, sub('dd'), sub('hh'), sub('mm'))
			if (isValidDate(date)) return date

			function sub(token) {
				var ch = 0
				if ((ch = format.indexOf(token)) > -1) {
					var n = Number(str.slice(ch, ch + token.length))
					return isNaN(n) ? 0 : n
				} else return 0
			}
		},

		dateToString = function (date, format) {
			if (!isValidDate(date)) return 'Invalid Date'
			return format
				.replace('yyyy', '' + date.getFullYear())
				.replace('MM', to2(date.getMonth() + 1))
				.replace('dd', to2(date.getDate()))
				.replace('hh', to2(date.getHours()))
				.replace('mm', to2(date.getMinutes()))
			function to2(n) { return (n < 10 ? '0' : '') + n }
		},

		on = function (el, e, callback) {
			listener ? el.addEventListener(e, callback) : el.attachEvent('on' + e, callback)
		}

	return DatePicker
})()