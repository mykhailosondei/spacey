import * as React from "react";
import type { SVGProps } from "react";
const SvgPlus = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      fill: "none",
      height: 32,
      width: 32,
      stroke: "#717171",
      strokeWidth: 2,
      overflow: "visible",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M2 16h28M16 2v28" />
  </svg>
);
export default SvgPlus;
