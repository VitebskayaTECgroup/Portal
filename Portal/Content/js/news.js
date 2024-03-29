﻿// News

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
	if (isLoading) return

	var news = document.getElementById('news')
	if (!news) return
	if (news.style.display == 'none') return

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
	watchNews(el)
})

function watchNews(el) {
	fetch(host + 'news/watch?id=' + el.getAttribute('news-id'), { method: 'POST' })
		.then(() => {
			el.classList.remove("news-unwatched")
			el.classList.remove("news-new")
			checkNewNews()
		})
}

function hideNews(icon, id) {
	fetch(host + 'news/hide/' + id)
		.then(() => {
			icon.closest('[news-id]').remove()
			checkNewNews()
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

function minimizeNews(el) {
	var newsEl = el.closest('[news-id]')
	watchNews(newsEl)
	newsEl.querySelector('.news-body').innerHTML = `<small class="news-preview" title="Нажмите, чтобы увидеть текст новости">посмотреть</small>`
}

function checkNewNews() {
	var newNews = document.querySelectorAll('.news-new')
	var link = document.querySelector('.news-new-highlight')
	if (!link) return
	link.style.display = (newNews && newNews.length) ? 'inline-block' : 'none'
}

function getNewNews() {
	if (isLoading) return
	isLoading = true

	var id = []
	document.querySelectorAll('[news-id]').forEach(function (el) {
		id.push(Number(el.getAttribute('news-id')))
	})

	fetch(host + 'news/list/?' + (id.length == 0 ? 'take=20' : ('pinned=false&before=' + Math.max.apply(Math, id))))
		.then(function (res) { return res.text() })
		.then(function (text) {
			var pinned = document.querySelectorAll('.news-pinned')
			if (pinned && pinned.length > 0) {
				var last = pinned[pinned.length - 1]
				last.insertAdjacentHTML('afterend', text)
			}
			else {
				document.getElementById('news').insertAdjacentHTML('afterbegin', text)
			}
			isLoading = false
			checkNewNews()
		})
}

function watchAllNews() {
	document.querySelectorAll('.news-unwatched').forEach(function (el) {
		watchNews(el)
		setTimeout(checkNewNews, 2500)
	})
}