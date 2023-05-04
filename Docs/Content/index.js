function action(url, params, callback) {
	let form = new FormData()
	for (var key in params) form.append(key, params[key])

	fetch(url, { method: 'POST', body: form })
		.then(res => res.json())
		.then(json => {
			if (json.Error) alert(json.Error)
			if (json.Done && callback) callback()
		})
}

function modal(url) {
	let overlay = document.getElementById('overlay')
	let el = document.getElementById('modal')
	if (url) {
		fetch(url, { method: 'GET' })
			.then(res => res.text())
			.then(text => {
				el.innerHTML = text
				overlay.classList.add('show')
				el.classList.add('show')
			})
	}
	else {
		el.classList.remove('show')
		overlay.classList.remove('show')
	}
}

function submit(url, selector, callback) {
	let params = {}
	document.querySelectorAll(selector + ' input, ' + selector + ' select, ' + selector + ' textarea').forEach(input => {
		let name = input.name
		let value = input.type == 'checkbox'
			? input.checked
			: input.type == 'file'
				? input.files[0]
				: input.value

		if (name.indexOf('[') < 0) {
			params[name] = value
		}
		else {
			if (!params[name].length) params[name] = [value]
			else params[name].join(value)
		}
	})

	let form = new FormData()
	for (var key in params) form.append(key, params[key])

	fetch(url, { method: 'POST', body: form })
		.then(res => res.json())
		.then(json => {
			if (json.Error) alert(json.Error)
			if (json.Done && callback) callback(json)
		})
}

function reform(key) {
	document.querySelectorAll('[reform]').forEach(el => {
		let keys = el.getAttribute('reform').split(',')
		el.style.display = keys.indexOf(key) < 0 ? 'none' : 'inherit'
	})
}