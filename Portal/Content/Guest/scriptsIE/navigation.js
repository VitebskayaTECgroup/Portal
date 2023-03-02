function openNavigationMenu() {
  var navigation = document.querySelector(".navigation");
  navigation.addEventListener("mouseover", function (event) {
    var navBlock = event.target.closest(".nav_block");
    if (navBlock) {
      var navLinks = navBlock.querySelector(".hidden");
      if (navLinks) {
        navLinks.classList.remove("hidden");
      }
    }
  });

  navigation.addEventListener("mouseout", function () {
    var navLinks = document.querySelectorAll(".nav_links");
    for (var i = 0; i < navLinks.length; i++) {
      navLinks[i].classList.add("hidden");
    }
  });
}

function openNavigationMenuForMobile() {
  var navigation = document.querySelector(".navigation_mobile");
  var louver = document.querySelector(".louver");
  var closeNavigation = navigation.querySelector(".close");
  closeNavigation.addEventListener("click", function () {
    navigation.style.display === "block"
      ? (navigation.style.display = "none")
      : (navigation.style.display = "block");
  });
  louver.addEventListener("click", function () {
    navigation.style.display === "block"
      ? (navigation.style.display = "none")
      : (navigation.style.display = "block");
  });
  navigation.addEventListener("click", function (event) {
    var prevOpenLinks = navigation.querySelector(".open");
    var navBlock = event.target.closest(".navigation_mobile__block");
    if (navBlock) {
      var navLinks = navBlock.querySelector(".navigation_mobile__links");
      if (navLinks) {
        navLinks.classList.toggle("open");
        if (prevOpenLinks && !prevOpenLinks.isEqualNode(navLinks)) {
          prevOpenLinks.classList.toggle("open");
        }
      }
    }
  });
}

openNavigationMenu();
openNavigationMenuForMobile();
