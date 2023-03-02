let allNewsInfo = [];
let IS_LOADING = false;
const STANDART_NUMBER_NEWS = 10;
const EXPANDED_NUMBER_NEWS = 50;
let ALL_DATA_WAS_RECEIVED = false;

function createColumnInfo(news) {
  const columnHeaderArrow = document.createElement("div");
  columnHeaderArrow.classList = "column_header__arrow";

  const columnHeaderTitle = document.createElement("div");
  columnHeaderTitle.classList = "column_header__title";
  columnHeaderTitle.textContent = news.Title;

  const columnInfoHeader = document.createElement("div");
  columnInfoHeader.classList = "column_info__header";
  columnInfoHeader.append(columnHeaderArrow, columnHeaderTitle);

  const dateNumber = /\d+/.exec(news.DateAdd);
  const options = {
    year: "numeric",
    month: "long",
    day: "numeric",
    timezone: "UTC",
  };
  const dateAdd = new Date(+dateNumber).toLocaleString("ru", options);
  const columnFooterTime = document.createElement("img");
  columnFooterTime.src = "/content/guest/img/time.png";
  columnFooterTime.alt = "Дата";
  columnFooterTime.loading = "lazy";
  columnFooterTime.classList = "column_footer__time";

  const columnFooterDate = document.createElement("div");
  columnFooterDate.classList = "column_footer__date";
  columnFooterDate.textContent = `${dateAdd}, Автор ${
    news.UserName === null ? "" : news.UserName
  }`;

  const columnInfoFooter = document.createElement("div");
  columnInfoFooter.classList = "column_info__footer";
  columnInfoFooter.append(columnFooterTime, columnFooterDate);
  columnHeaderTitle.append(columnInfoFooter);

  const columnInfo = document.createElement("div");
  columnInfo.classList = "column_info";
  columnInfo.append(columnInfoHeader);
  return columnInfo;
}

function createColumnMessage() {
  const columnLinks = document.createElement("div");
  columnLinks.classList = "column_message__links";

  const columnMessageInfo = document.createElement("div");
  columnMessageInfo.classList = "column_message__info";

  const columnMessage = document.createElement("div");
  columnMessage.classList = "column_message";
  columnMessage.style.display = "none";
  columnMessage.append(columnMessageInfo, columnLinks);

  return columnMessage;
}

function addColumnsNews(data) {
  const columns = document.querySelector(".columns");
  const allNews = data.News;
  allNews.forEach((news) => {
    const column = document.createElement("div");
    column.classList = "column";
    column.append(createColumnInfo(news), createColumnMessage(news));
    columns.append(column);
  });
}

function showColumnMessage() {
  const columns = document.querySelector(".columns");
  columns.addEventListener("click", (event) => {
    const target = event.target;
    const columnInfo = event.target.closest(".column_info");
    if (columnInfo) {
      const allColumnInfo = document.querySelectorAll(".column_info");
      const indexCurrentColumn = Array.from(allColumnInfo).indexOf(columnInfo);
      const column = columnInfo.closest(".column");
      const columnHeaderArrow = column.querySelector(".column_header__arrow");

      let columnMessage = column.querySelector(".column_message");
      let columnMessageInfo = column.querySelector(".column_message__info");
      let columnMessageLinks = column.querySelector(".column_message__links");

      if (columnMessage.style.display === "none") {
        columnMessage.style.display = "block";
        if (!columnMessageInfo.innerHTML) {
          columnMessageInfo.innerHTML = allNewsInfo[indexCurrentColumn].Message;
        }
        if (!columnMessageLinks.innerHTML) {
          const links = allNewsInfo[indexCurrentColumn].Links.split(";");
          links.forEach((link) => {
            const columnMessageLink = document.createElement("a");
            columnMessageLink.href = `http://www.vst.vitebsk.energo.net/files/Новости/${allNewsInfo[indexCurrentColumn].Id}/${link}`;
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
  const spinner = document.querySelector(".spinner-wave");
  show ? (spinner.style.display = "block") : (spinner.style.display = "none");
}

function throttle(callee, timeout) {
  let timer = null;

  return function perform() {
    if (timer) return;
    timer = setTimeout(() => {
      callee();

      clearTimeout(timer);
      timer = null;
    }, timeout);
  };
}

function loadNewsOnScroll() {
  const skipCountNews = document.querySelectorAll(".column").length;
  const height = document.body.offsetHeight;
  const screenHeight = window.innerHeight;
  const scrolled = window.scrollY;
  const threshold = height - screenHeight;
  const position = scrolled + screenHeight;

  if (position >= threshold) {
    if (IS_LOADING) return;
    IS_LOADING = true;
    getDataNews(EXPANDED_NUMBER_NEWS - 1, skipCountNews);
  }
}

function getDataNews(count = STANDART_NUMBER_NEWS, skip = 0) {
  if (!ALL_DATA_WAS_RECEIVED) {
    fetch(
      `http://www.vst.vitebsk.energo.net/guest/getnews?count=${count}&skip=${skip}`
    )
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        let AMOUNT_ALL_NEWS = allNewsInfo.length;
        data.News.forEach((news) => allNewsInfo.push(news));
        AMOUNT_ALL_NEWS !== allNewsInfo.length
          ? addColumnsNews(data)
          : showSpinnerOnPage(false);
      })
      .then(() => {
        IS_LOADING = false;
      });
  }
}

showColumnMessage();
activeNewsButtons();
getDataNews();
