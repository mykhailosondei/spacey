import * as React from "react";
import type { SVGProps } from "react";
const SvgExclude = (props: SVGProps<SVGSVGElement>) => (
  <svg
      style={{width: "2em", height: "2em", fill: "none"}}
    xmlns="http://www.w3.org/2000/svg"
    width="1em"
    height="1em"
    fill="none"
    viewBox="0 0 65 63"
    {...props}
  >
    <path
      fill="rgb(85, 36, 139)"
      fillRule="evenodd"
      d="M23.895 3.822a12.403 12.403 0 0 1 17.21-.1l19.5 18.509a12.5 12.5 0 0 1 3.895 9.066v23.704a7.5 7.5 0 0 1-7.5 7.5H8a7.5 7.5 0 0 1-7.5-7.5V31.297a12.5 12.5 0 0 1 3.895-9.066l19.5-18.509ZM37.663 7.35a7.5 7.5 0 0 0-10.326 0l-19.5 18.508a7.5 7.5 0 0 0-2.337 5.44v23.704a2.5 2.5 0 0 0 2.5 2.5h49a2.5 2.5 0 0 0 2.5-2.5V31.297a7.5 7.5 0 0 0-2.337-5.44z"
      clipRule="evenodd"
    />
  </svg>
);
export default SvgExclude;
