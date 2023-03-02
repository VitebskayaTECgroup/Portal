function startSlider() {
  const nextButton = document.querySelector(".sim-slider-arrow-right");
  const prevButton = document.querySelector(".sim-slider-arrow-left");
  const allImageSlider = document.querySelectorAll(".slider_image");
  const allSliderSwitches = document.querySelectorAll(".slider_switcher");
  const carousel = document.querySelector(".carousel");

  let currentItemNumber = 0;

  prevButton.addEventListener("click", () => {
    allSliderSwitches[currentItemNumber].classList.remove(
      "slider_switcher__active"
    );

    currentItemNumber -= 1;
    if (currentItemNumber < 0) {
      currentItemNumber = allImageSlider.length - 1;
    }

    const newImageSlider = allImageSlider[currentItemNumber];
    const newImageWidth = newImageSlider.scrollWidth;
    carousel.style.transform = `translate(-${
      newImageWidth * currentItemNumber
    }px, 0)`;

    allSliderSwitches[currentItemNumber].classList.add(
      "slider_switcher__active"
    );
  });

  nextButton.addEventListener("click", () => {
    allSliderSwitches[currentItemNumber].classList.remove(
      "slider_switcher__active"
    );

    currentItemNumber += 1;
    if (currentItemNumber > allImageSlider.length - 1) {
      currentItemNumber = 0;
    }

    const newImageSlider = allImageSlider[currentItemNumber];
    const newImageWidth = newImageSlider.scrollWidth;
    carousel.style.transform = `translate(-${
      newImageWidth * currentItemNumber
    }px, 0)`;

    allSliderSwitches[currentItemNumber].classList.add(
      "slider_switcher__active"
    );
  });

  allSliderSwitches.forEach((sliderSwitcher) => {
    sliderSwitcher.addEventListener("click", (event) => {
      const target = event.target;
      allSliderSwitches[currentItemNumber].classList.remove(
        "slider_switcher__active"
      );

      currentItemNumber = +target.dataset.switcher;
      const newImageSlider = allImageSlider[currentItemNumber];
      const newImageWidth = newImageSlider.scrollWidth;
      carousel.style.transform = `translate(-${
        newImageWidth * currentItemNumber
      }px, 0)`;

      allSliderSwitches[currentItemNumber].classList.add(
        "slider_switcher__active"
      );
    });
  });
}

startSlider();
