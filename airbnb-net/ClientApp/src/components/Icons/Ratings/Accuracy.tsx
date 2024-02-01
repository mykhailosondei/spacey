import * as React from "react";
import type { SVGProps } from "react";
const SvgAccuracy = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      height: 32,
      width: 32,
      fill: "currentcolor",
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M16 1a15 15 0 1 1 0 30 15 15 0 0 1 0-30m0 2a13 13 0 1 0 0 26 13 13 0 0 0 0-26m7 7.59L24.41 12 13.5 22.91 7.59 17 9 15.59l4.5 4.5z" />
  </svg>
);
export default SvgAccuracy;
