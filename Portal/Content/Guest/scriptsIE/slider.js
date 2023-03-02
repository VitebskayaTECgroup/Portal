function startSlider() {
  var nextButton = document.querySelector(".sim-slider-arrow-right");
  var prevButton = document.querySelector(".sim-slider-arrow-left");
  var allImageSlider = document.querySelectorAll(".slider_image");
  var allSliderSwitches = document.querySelectorAll(".slider_switcher");
  var carousel = document.querySelector(".carousel");

  var currentItemNumber = 0;

  prevButton.addEventListener("click", function () {
    allSliderSwitches[currentItemNumber].classList.remove(
      "slider_switcher__active"
    );

    currentItemNumber -= 1;
    if (currentItemNumber < 0) {
      currentItemNumber = allImageSlider.length - 1;
    }

    var newImageSlider = allImageSlider[currentItemNumber];
    var newImageWidth = newImageSlider.scrollWidth;
    carousel.style.transform =
      "translate(-" + newImageWidth * currentItemNumber + "px, 0)";

    allSliderSwitches[currentItemNumber].classList.add(
      "slider_switcher__active"
    );
  });

  nextButton.addEventListener("click", function () {
    allSliderSwitches[currentItemNumber].classList.remove(
      "slider_switcher__active"
    );

    currentItemNumber += 1;
    if (currentItemNumber > allImageSlider.length - 1) {
      currentItemNumber = 0;
    }

    var newImageSlider = allImageSlider[currentItemNumber];
    var newImageWidth = newImageSlider.scrollWidth;

    carousel.style.transform =
      "translate(-" + newImageWidth * currentItemNumber + "px, 0)";

    allSliderSwitches[currentItemNumber].classList.add(
      "slider_switcher__active"
    );
  });

  for (var i = 0; i < allSliderSwitches.length; i++) {
    allSliderSwitches[i].getAttribute("data-switcher", i);
    allSliderSwitches[i].addEventListener("click", function (event) {
      var target = event.target;
      allSliderSwitches[currentItemNumber].classList.remove(
        "slider_switcher__active"
      );

      currentItemNumber = +target.getAttribute("data-switcher");
      var newImageSlider = allImageSlider[currentItemNumber];
      var newImageWidth = newImageSlider.scrollWidth;
      carousel.style.transform =
        "translate(-" + newImageWidth * currentItemNumber + "px, 0)";

      allSliderSwitches[currentItemNumber].classList.add(
        "slider_switcher__active"
      );
    });
  }
}

startSlider();
