import * as React from "react";
import type { SVGProps } from "react";
const SvgWheelchairAccessible = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      height: 24,
      width: 24,
      fill: "currentcolor",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M11 23H5v4h20.59L2.29 3.7l1.42-1.4L27 25.58V5h-4v6h-8V9h6V3h8v24.59l.7.7-1.4 1.42-.72-.71H3v-8h6v-6h2z" />
  </svg>
);
export default SvgWheelchairAccessible;
