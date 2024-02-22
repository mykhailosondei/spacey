import * as React from "react";
import type { SVGProps } from "react";
const SvgNoBookings = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      height: 32,
      width: 32,
      fill: "#222",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M24 1a5 5 0 0 1 5 4.78v5.31h-2V6a3 3 0 0 0-2.82-3H8a3 3 0 0 0-3 2.82V26a3 3 0 0 0 2.82 3h5v2H8a5 5 0 0 1-5-4.78V6a5 5 0 0 1 4.78-5H8zm-2 12a9 9 0 1 1 0 18 9 9 0 0 1 0-18m0 2a7 7 0 1 0 0 14 7 7 0 0 0 0-14m3.02 3.17 1.36 1.46-6.01 5.64-3.35-3.14 1.36-1.46 1.99 1.86z" />
  </svg>
);
export default SvgNoBookings;
