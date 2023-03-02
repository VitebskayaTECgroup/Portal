function openNavigationMenu() {
  const navigation = document.querySelector(".navigation");
  navigation.addEventListener("mouseover", (event) => {
    const target = event.target;
    const navBlock = event.target.closest(".nav_block");
    if (navBlock) {
      const navLinks = navBlock.querySelector(".hidden");
      if (navLinks) {
        navLinks.classList.remove("hidden");
      }
    }
  });
  navigation.addEventListener("mouseout", () => {
    const navLinks = document.querySelectorAll(".nav_links");
    navLinks.forEach((item) => {
      item.classList.add("hidden");
    });
  });
}

function openNavigationMenuForMobile() {
  const navigation = document.querySelector(".navigation_mobile");
  const louver = document.querySelector(".louver");
  const closeNavigation = navigation.querySelector(".close");

  closeNavigation.addEventListener("click", () => {
    navigation.style.display === "block"
      ? (navigation.style.display = "none")
      : (navigation.style.display = "block");
  });

  louver.addEventListener("click", () => {
    navigation.style.display === "block"
      ? (navigation.style.display = "none")
      : (navigation.style.display = "block");
  });

  navigation.addEventListener("click", (event) => {
    const target = event.target;
    const prevOpenLinks = navigation.querySelector(".open");
    const navBlock = target.closest(".navigation_mobile__block");

    if (navBlock) {
      const navLinks = navBlock.querySelector(".navigation_mobile__links");
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

