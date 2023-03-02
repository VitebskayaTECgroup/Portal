var allNewsInfo = [];
var IS_LOADING = false;
var STANDART_NUMBER_NEWS = 10;
var EXPANDED_NUMBER_NEWS = 50;
var ALL_DATA_WAS_RECEIVED = false;

function createColumnInfo(news) {
  var columnHeaderArrow = document.createElement("div");
  columnHeaderArrow.className += " column_header__arrow";

  var columnHeaderTitle = document.createElement("div");
  columnHeaderTitle.className += " column_header__title";
  columnHeaderTitle.textContent = news.Title;

  var columnInfoHeader = document.createElement("div");
  columnInfoHeader.className += " column_info__header";
  columnInfoHeader.append(columnHeaderArrow, columnHeaderTitle);

  var dateNumber = /\d+/.exec(news.DateAdd);
  var options = {
    year: "numeric",
    month: "long",
    day: "numeric",
    timezone: "UTC",
  };
  var dateAdd = new Date(+dateNumber).toLocaleString("ru", options);
  var columnFooterTime = document.createElement("img");
  columnFooterTime.src = "./img/time.png";
  columnFooterTime.alt = "Дата";
  columnFooterTime.loading = "lazy";
  columnFooterTime.className += " column_footer__time";

  var columnFooterDate = document.createElement("div");
  var userName = news.UserName === null ? "" : news.UserName;
  columnFooterDate.className += " column_footer__date";
  columnFooterDate.textContent = dateAdd + ", Автор " + userName;

  var columnInfoFooter = document.createElement("div");
  columnInfoFooter.className += " column_info__footer";
  columnInfoFooter.append(columnFooterTime, columnFooterDate);
  columnHeaderTitle.append(columnInfoFooter);

  var columnInfo = document.createElement("div");
  columnInfo.className += " column_info";
  columnInfo.append(columnInfoHeader);
  return columnInfo;
}

function createColumnMessage() {
  var columnLinks = document.createElement("div");
  columnLinks.className += " column_message__links";
  var columnMessageInfo = document.createElement("div");
  columnMessageInfo.className += " column_message__info";
  var columnMessage = document.createElement("div");
  columnMessage.className += " column_message";
  columnMessage.style.display = "none";
  columnMessage.append(columnMessageInfo, columnLinks);
  return columnMessage;
}

function addColumnsNews(data) {
  var columns = document.querySelector(".columns");
  var allNews = data.News;
  allNews.forEach(function (news) {
    var column = document.createElement("div");
    column.className += " column";
    column.append(createColumnInfo(news), createColumnMessage(news));
    columns.append(column);
  });
}

function showColumnMessage() {
  var columns = document.querySelector(".columns");
  columns.addEventListener("click", function (event) {
    var columnInfo = event.target.closest(".column_info");
    if (columnInfo) {
      var allColumnInfo = document.querySelectorAll(".column_info");
      var indexCurrentColumn = Array.from(allColumnInfo).indexOf(columnInfo);
      var column = columnInfo.closest(".column");
      var columnHeaderArrow = column.querySelector(".column_header__arrow");
      var columnMessage = column.querySelector(".column_message");
      var columnMessageInfo = column.querySelector(".column_message__info");
      var columnMessageLinks = column.querySelector(".column_message__links");

      if (columnMessage.style.display === "none") {
        columnMessage.style.display = "block";

        if (
          !columnMessageInfo.innerHTML &&
          allNewsInfo[indexCurrentColumn].Message !== null
        ) {
          columnMessageInfo.innerHTML = allNewsInfo[indexCurrentColumn].Message;
        }

        if (!columnMessageLinks.innerHTML) {
          var links = allNewsInfo[indexCurrentColumn].Links.split(";");
          links.forEach(function (link) {
            var columnMessageLink = document.createElement("a");
            columnMessageLink.href =
              "http://www.vst.vitebsk.energo.net/files/Новости/" +
              allNewsInfo[indexCurrentColumn].Id +
              "/" +
              link;
            columnMessageLink.textContent = link;
            columnMessageLinks.append(columnMessageLink);
          });
        }
        columnHeaderArrow.classList.add("rotate");
        columnInfo.classList.add("column_info__active");
      } else {
        columnMessage.style.display = "none";
        columnHeaderArrow.classList.remove("rotate");
        columnInfo.classList.remove("column_info__active");
      }
    }
  });
}

function activeNewsButtons() {
  window.addEventListener("scroll", throttle(loadNewsOnScroll, 250));
  showSpinnerOnPage(true);
}

function showSpinnerOnPage(show) {
  var spinner = document.querySelector(".spinner-wave");
  show ? (spinner.style.display = "block") : (spinner.style.display = "none");
}

function throttle(callee, timeout) {
  var timer = null;
  return function perform() {
    if (timer) return;
    timer = setTimeout(function () {
      callee();
      clearTimeout(timer);
      timer = null;
    }, timeout);
  };
}

function loadNewsOnScroll() {
  var skipCountNews = document.querySelectorAll(".column").length;
  var height = document.body.offsetHeight;
  var screenHeight = window.innerHeight;
  var scrolled = window.pageYOffset;
  var threshold = height - screenHeight;
  var position = scrolled + screenHeight;

  if (position >= threshold) {
    if (IS_LOADING) return;
    IS_LOADING = true;
    getDataNews(EXPANDED_NUMBER_NEWS - 1, skipCountNews);
  }
}

function getDataNews(count, skip) {
  if (!count) {
    count = STANDART_NUMBER_NEWS;
  }
  if (!skip) {
    skip = 0;
  }
  if (!ALL_DATA_WAS_RECEIVED) {
    fetch(
      "http://www.vst.vitebsk.energo.net/guest/getnews?count=" +
        count +
        "&skip=" +
        skip
    )
      .then(function (response) {
        return response.json();
      })
      .then(function (data) {
        var AMOUNT_ALL_NEWS = allNewsInfo.length;
        data.News.forEach(function (news) {
          allNewsInfo.push(news);
        });
        AMOUNT_ALL_NEWS !== allNewsInfo.length
          ? addColumnsNews(data)
          : showSpinnerOnPage(false);
      })
      .then(function () {
        IS_LOADING = false;
      });
  }
}

showColumnMessage();
activeNewsButtons();
getDataNews();
