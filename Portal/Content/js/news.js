// News

document.addEventListener('click', function (ev) {
	var el = ev.target
	if (el.classList.contains('news-preview')) {
		var body = el.parentNode
		var id = body.closest('[news-id]').getAttribute('news-id')
		fetch(host + 'news/body/?id=' + id)
			.then(function (res) { return res.text() })
			.then(function (text) {
				body.innerHTML = text
			})
	}
})

var isLoading = false

window.addEventListener('scroll', function () {
	if (!document.getElementById('news')) return
	if (isLoading) return

	var scrollHeight = Math.max(
		document.body.scrollHeight, document.documentElement.scrollHeight,
		document.body.offsetHeight, document.documentElement.offsetHeight,
		document.body.clientHeight, document.documentElement.clientHeight
	)
	if (scrollHeight - this.window.pageYOffset < 1200) {
		isLoading = true

		var news = document.querySelectorAll('[news-id]')
		var last = news.length > 0 ? (news[news.length - 1]).getAttribute('news-id') : 0

		fetch(host + 'news/list/?take=10&skip=' + last)
			.then(function (res) { return res.text() })
			.then(function (text) {
				document.getElementById('news').insertAdjacentHTML('beforeend', text)
				isLoading = false
			})
	}
})

document.addEventListener('click', function (ev) {
	var el = ev.target.closest('.news-unwatched')
	if (!el) return
	fetch(host + 'news/watch?id=' + el.getAttribute('news-id'), { method: 'POST' })
		.then(res => res.json())
		.then(json => {
			if (json.Done) {
				el.classList.remove("news-unwatched")
			}
		})
})

function hideNews(icon, id) {
	fetch(host + 'news/hide/' + id)
		.then(() => {
			icon.closest('[news-id]').remove()
		})
}

function showNews(id) {
	fetch(host + 'news/show/' + id)
		.then(() => {
			location.reload()
		})
}

function pinNews(id) {
	fetch(host + 'news/pin/' + id)
		.then(() => {
			location.reload()
		})
}

function unpinNews(id) {
	fetch(host + 'news/unpin/' + id)
		.then(() => {
			location.reload()
		})
}